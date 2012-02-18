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
        private int _sequentialFrameId = 0xff;

        public LogLevel LogLevel
        {
            get { return Logger.LoggingLevel; }
            set { Logger.LoggingLevel = value; }
        }

        public XBeeConfiguration Config { get; private set; }

        protected XBee()
        {
            _parser = new PacketParser();
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

        public AtResponse Send(AtCmd atCommand, int[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return Send(new AtCommand(atCommand, value), timeout);
        }

        public AtResponse Send(AtCommand atCommand, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtResponse)Send(atCommand, typeof(AtResponse), timeout);
        }

        public RemoteAtResponse Send(AtCmd atCommand, XBeeAddress16 remoteXbee, int[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return Send(new RemoteAtCommand(atCommand, remoteXbee, value), timeout);
        }

        public RemoteAtResponse Send(AtCmd atCommand, XBeeAddress64 remoteXbee, int[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return Send(new RemoteAtCommand(atCommand, remoteXbee, value), timeout);
        }

        public RemoteAtResponse Send(RemoteAtCommand remoteAtCommand, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse)Send(remoteAtCommand, typeof(RemoteAtResponse), timeout);
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
            if (xbeeRequest.FrameId == XBeeRequest.NO_RESPONSE_FRAME_ID)
                throw new XBeeException("Frame Id cannot be 0 for a synchronous call -- it will always timeout as there is no response!");

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

        public void SendAsync(XBeeRequest request)
        {
            if (Config != null)
            {
                if (Config.HardwareVersion == HardwareVersions.SERIES1 && request is IZigbeePacket)
                    throw new ArgumentException("You are connected to a Series 1 radio but attempting to send Series 2 requests");

                if (Config.HardwareVersion == HardwareVersions.SERIES2 && request is IWpanPacket)
                    throw new ArgumentException("You are connected to a Series 2 radio but attempting to send Series 1 requests");
            }

            if (Logger.IsActive(LogLevel.Debug))
                Logger.Debug("Sending " + request.GetType().Name + ": " + request);
            
            SendPacket(request.GetXBeePacket());
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

        public void SendPacket(int[] packet)
        {
            Logger.LowDebug("sending packet " + ByteUtils.ToBase16(packet));
            _connection.Send(Arrays.ToByteArray(packet));
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

        public int GetCurrentFrameId()
        {
            return _sequentialFrameId;
        }

        /// <summary>
        /// This is useful for obtaining a frame id when composing your XBeeRequest. 
        /// It will return frame ids in a sequential manner until the maximum is reached (0xff)
        /// and it flips to 1 and starts over.
        /// </summary>
        /// <returns></returns>
        public int GetNextFrameId()
        {
            if (_sequentialFrameId == 0xff)
            {
                // flip
                _sequentialFrameId = 1;
            }
            else
            {
                _sequentialFrameId++;
            }

            return _sequentialFrameId;
        }

        /// <summary>
        /// Updates the frame id.
        /// </summary>
        /// <param name="val">Any value between 1 and ff is valid</param>
        public void UpdateFrameId(int val)
        {
            if (val <= 0 || val > 0xff)
                throw new ArgumentException("invalid frame id");
        
            _sequentialFrameId = val;
        }

        public bool IsConnected()
        {
            return _connection != null && _connection.Connected;
        }

        public XBeeResponse[] CollectResponses(int wait, int maxPacketCount)
        {
            return CollectResponses(wait, null, new CountLimitTerminator(maxPacketCount));
        }

        public XBeeResponse[] CollectResponses(int wait, Type packetType, int maxPacketCount)
        {
            return CollectResponses(wait, packetType, new CountLimitTerminator(maxPacketCount));
        }

        /// <summary>
        /// Collects responses until the timeout is reached or the CollectTerminator returns true
        /// </summary>
        /// <param name="wait"></param>
        /// <param name="packetType"></param>
        /// <param name="terminator"></param>
        /// <returns></returns>
        public XBeeResponse[] CollectResponses(int wait, Type packetType = null, ICollectTerminator terminator = null)
        {
            var result = new MultiplePacketListener(packetType, terminator);

            try
            {
                AddPacketListener(result);
                return result.GetPackets(wait);
            }
            finally
            {
                RemovePacketListener(result);
            }
        }
    }
}