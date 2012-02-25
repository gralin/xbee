using System;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Api.Wpan;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// This is an API for communicating with Digi XBee 802.15.4 and ZigBee radios
    /// </summary>
    public class XBee
    {
        private readonly IXBeeConnection _connection;
        private readonly PacketParser _parser;
        private readonly PacketIdGenerator _idGenerator;

        public XBeeConfiguration Config { get; private set; }

        public bool IsConnected()
        {
            return _connection != null && _connection.Connected;
        }

        protected XBee()
        {
            _parser = new PacketParser();
            _idGenerator = new PacketIdGenerator();
        }

        public XBee(IXBeeConnection connection) 
            : this()
        {
            _connection = connection;
            _connection.DataReceived += (data, offset, count) =>
            {
                var buffer = new byte[count];
                Array.Copy(data, offset, buffer, 0, count);
                _parser.AddToParse(buffer);
            };
        }

        public XBee(string portName, int baudRate)
            : this(new SerialConnection(portName, baudRate))
        {
        }

        public void Open()
        {
            _parser.Start();
            _connection.Open();
            ReadConfiguration();
        }

        public void Close()
        {
            _parser.Stop();
            _connection.Close();
        }

        public void ReadConfiguration()
        {
            try
            {
                Config = XBeeConfiguration.Read(this);

                if (Config.ApiMode != ApiModes.EnabledWithEscaped)
                {
                    Logger.LowDebug("XBee radio is in API mode without escape characters (AP=1)."
                                    + " The radio must be configured in API mode with escape bytes "
                                    + "(AP=2) for use with this library.");

                    Config.SetApiMode(ApiModes.EnabledWithEscaped);
                    Config.Save();

                    Logger.Debug("Successfully set AP mode to ApiMode.EnabledWithEscaped");
                }

                if (!Logger.IsActive(LogLevel.Info))
                    return;
                
                Logger.Info(Config.ToString());
            }
            catch (XBeeTimeoutException)
            {
                throw new XBeeException("AT command timed-out while attempt to read configuration. "
                    + "The XBee radio must be in API mode (AP=2) to use with this library");
            }
        }

        public void AddPacketListener(IPacketListener listener)
        {
            _parser.AddPacketListener(listener);
        }

        public void RemovePacketListener(IPacketListener listener)
        {
            _parser.RemovePacketListener(listener);
        }

        // Creating requests

        public XBeeRequest CreateRequest(XBeeAddress destination, string payload)
        {
            return CreateRequest(destination, Arrays.ToByteArray(payload));
        }

        public XBeeRequest CreateRequest(XBeeAddress destination, byte[] payload)
        {
            return Config.IsSeries1()
                ? (XBeeRequest)new TxRequest(destination, payload) { FrameId = _idGenerator.GetNext() }
                : new ZNetTxRequest(destination, payload) { FrameId = _idGenerator.GetNext() };
        }

        public XBeeRequest CreateRequest(AtCmd atCommand, byte[] value = null)
        {
            return new AtCommand(atCommand, value) { FrameId = _idGenerator.GetNext() };
        }

        public XBeeRequest CreateRequest(AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null)
        {
            return new RemoteAtCommand(atCommand, remoteXbee, value) { FrameId = _idGenerator.GetNext() };
        }

        // Sending requests

        public XBeeResponse Send(XBeeAddress destination, byte[] payload)
        {
            var request = CreateRequest(destination, payload);

            return request is TxRequest
                ? Send(request, typeof(TxStatusResponse))
                : Send(request, typeof(ZNetTxStatusResponse));
        }

        public XBeeResponse Send(XBeeAddress destination, string payload)
        {
            return Send(destination, Arrays.ToByteArray(payload));
        }

        public AtResponse Send(AtCmd atCommand, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtResponse)Send(CreateRequest(atCommand, value), typeof(AtResponse), timeout);
        }

        public RemoteAtResponse Send(AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse)Send(CreateRequest(atCommand, remoteXbee, value), typeof(RemoteAtResponse), timeout);
        }

        /// <summary>
        /// Synchronous method for sending an XBeeRequest and obtaining the 
        /// corresponding response (response that has same frame id).
        /// <para>
        /// This method returns the first response object with a matching frame id, within the timeout
        /// period, so it is important to use a unique frame id (relative to previous subsequent requests).
        /// </para>
        /// <para>
        /// This method must only be called with requests that receive a response of
        /// type XBeeFrameIdResponse.  All other request types will timeout.
        /// </para>
        /// <para>
        /// Keep in mind responses received here will also be available through the getResponse method
        /// and the packet listener.  If you would prefer to not have these responses added to the response queue,
        /// you can add a ResponseQueueFilter via XBeeConfiguration to ignore packets that are sent in response to
        /// a request.  Another alternative is to call clearResponseQueue prior to calling this method.
        /// </para>
        /// <para>
        /// It is recommended to use a timeout of at least 5 seconds, since some responses can take a few seconds or more
        /// (e.g. if remote radio is not powered on).
        /// </para>
        /// </summary>
        /// <remarks>
        /// This method is thread-safe 
        /// </remarks>
        /// <param name="xbeeRequest"></param>
        /// <param name="expectedResponse"></param>
        /// <param name="timeout"></param>
        /// <exception cref="XBeeTimeoutException">
        /// XBeeTimeoutException thrown if no matching response is identified
        /// </exception>
        /// <returns></returns>
        public XBeeResponse Send(XBeeRequest xbeeRequest, Type expectedResponse = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            if (xbeeRequest.FrameId == PacketIdGenerator.NoResponseId)
            {
                SendAsync(xbeeRequest);
                return null;
            }

            var listener = new SinglePacketListener(expectedResponse);

            try
            {
                AddPacketListener(listener);
                SendAsync(xbeeRequest);
                return listener.GetPacket(timeout);
            }
            finally
            {
                _parser.RemovePacketListener(listener);
            }
        }

        public void SendAsync(AtCmd atCommand, byte[] value = null)
        {
            SendAsync(new AtCommand(atCommand, value));
        }

        public void SendAsync(XBeeRequest request)
        {
            if (Config != null)
            {
                if (Config.IsSeries1() && request is IZigbeePacket)
                    throw new ArgumentException("You are connected to a Series 1 radio but attempting to send Series 2 requests");

                if (Config.IsSeries2() && request is IWpanPacket)
                    throw new ArgumentException("You are connected to a Series 2 radio but attempting to send Series 1 requests");
            }

            if (Logger.IsActive(LogLevel.Debug))
                Logger.Debug("Sending " + request.GetType().Name + ": " + request);
            
            SendPacket(request.GetXBeePacket());
        }

        public void SendAsync(XBeeAddress destination, byte[] payload)
        {
            if (Config.IsSeries1())
            {
                SendAsync(new TxRequest(destination, payload));
            }
            else
            {
                SendAsync(new ZNetTxRequest(destination, payload));
            }
        }

        public void SendAsync(XBeeAddress destination, string payload)
        {
            SendAsync(destination, Arrays.ToByteArray(payload));
        }

        /// <summary>
        /// It's possible for packets to get interspersed if multiple threads send simultaneously.  
        /// This method is not thread-safe because doing so would introduce a synchronized performance penalty 
        /// for the vast majority of users that will not never need thread safety.
        /// That said, it is responsibility of the user to provide synchronization if multiple threads are sending.
        /// </summary>
        /// <remarks>
        /// Not thread safe.
        /// </remarks>
        /// <param name="packet"></param>
        public void SendPacket(XBeePacket packet)
        {
            SendPacket(packet.ToByteArray());
        }

        public void SendPacket(byte[] packet)
        {
            Logger.LowDebug("sending packet " + ByteUtils.ToBase16(packet));
            _connection.Send(packet);
        }

        public IAsyncResult BeginSend(XBeeRequest request, IPacketListener responseListener)
        {
            AddPacketListener(responseListener);
            SendAsync(request);
            return new AsyncSendResult(request, responseListener);
        }

        // Receiving responses

        public XBeeResponse[] EndReceive(IAsyncResult asyncResult, int timeout = -1)
        {
            var asyncSendResult = asyncResult as AsyncSendResult;

            if (asyncSendResult == null)
                throw new ArgumentException("invalid asyncResult");

            var responseListener = asyncSendResult.ResponseListener;

            if (responseListener == null)
                throw new ArgumentException("asyncResult is missing response listener");

            try
            {
                return responseListener.GetPackets(timeout);
            }
            finally 
            {
                RemovePacketListener(responseListener);
            }
        }

        public XBeeResponse Receive(Type expectedType = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            var listener = new SinglePacketListener(expectedType);

            try
            {
                AddPacketListener(listener);
                return listener.GetPacket(timeout);
            }
            finally
            {
                _parser.RemovePacketListener(listener);
            }
        }

        public XBeeResponse[] CollectResponses(int timeout = -1, byte maxPacketCount = byte.MaxValue, Type packetType = null)
        {
            var validator = packetType != null ? new TypeValidator(packetType) : null;
            var terminator = maxPacketCount != byte.MaxValue ? new CountLimitTerminator(maxPacketCount) : null;
            return CollectResponses(timeout, validator, terminator);
        }

        public XBeeResponse[] CollectResponses(int timeout = -1, IPacketValidator validator = null, IPacketTerminator terminator = null)
        {
            var listener = new PacketListener(terminator, validator);

            try
            {
                AddPacketListener(listener);
                return listener.GetPackets(timeout);
            }
            finally
            {
                RemovePacketListener(listener);
            }
        }
    }
}