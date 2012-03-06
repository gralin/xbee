using System;
using System.Collections;
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
        
        private AddressLookupListener _addressLookupListener;
        private bool _addressLookupEnabled;

        private DataReceivedListener _dataReceivedListener;
        private bool _dataReceivedEventEnabled;

        public Hashtable AddressLookup { get; private set; }
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
            EnableDataReceivedEvent();
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

        public void EnableAddressLookup()
        {
            if (_addressLookupEnabled)
                return;

            if (AddressLookup == null)
                AddressLookup = new Hashtable();

            if (_addressLookupListener == null)                 
                _addressLookupListener = new AddressLookupListener(AddressLookup);
            
            AddPacketListener(_addressLookupListener);
            _addressLookupEnabled = true;
        }

        public void DisableAddressLookup()
        {
            if (!_addressLookupEnabled)
                return;

            AddressLookup.Clear();
            RemovePacketListener(_addressLookupListener);
            _addressLookupEnabled = false;
        }

        public void EnableDataReceivedEvent()
        {
            if (_dataReceivedEventEnabled)
                return;

            if (_dataReceivedListener == null)
                _dataReceivedListener = new DataReceivedListener(this);

            AddPacketListener(_dataReceivedListener);
            _dataReceivedEventEnabled = true;
        }

        public void DisableDataReceivedEvent()
        {
            if (!_dataReceivedEventEnabled)
                return;

            RemovePacketListener(_dataReceivedListener);
            _dataReceivedEventEnabled = false;
        }

        public NodeInfo[] DiscoverNodes()
        {
            var discoveryTimeout = Send(AtCmd.NodeDiscoverTimeout);

            // it seems that Zigbee modules have longer timeout (2 bytes)
            int timeout = discoveryTimeout.Value.Length == 1
                              ? discoveryTimeout.Value[0]
                              : UshortUtils.ToUshort(discoveryTimeout.Value);

            // ms + 1 extra second
            timeout = timeout * 100 + 1000;

            var request = CreateRequest(AtCmd.NodeDiscover);
            var responses = BeginSend(request, new NodeDiscoveryListener()).EndReceive(timeout);
            var result = new NodeInfo[responses.Length];

            for (var i = 0; i < responses.Length; i++)
            {
                var foundNode = Config.IsSeries1()
                    ? (NodeInfo)Wpan.NodeDiscover.Parse(responses[i])
                    : Zigbee.NodeDiscover.Parse(responses[i]);

                if (_addressLookupEnabled)
                    AddressLookup[foundNode.SerialNumber] = foundNode.NetworkAddress;

                result[i] = foundNode;
            }

            return result;
        }

        // Creating requests

        public XBeeRequest CreateRequest(string payload, XBeeAddress destination = null)
        {
            return CreateRequest(Arrays.ToByteArray(payload), destination);
        }

        public XBeeRequest CreateRequest(byte[] payload, XBeeAddress destination = null)
        {
            if (Config.IsSeries1())
            {
                return new Wpan.TxRequest(destination ?? XBeeAddress16.Broadcast, payload) {FrameId = _idGenerator.GetNext()};
            }
            else
            {
                return new Zigbee.TxRequest(destination ?? XBeeAddress16.ZnetBroadcast, payload) {FrameId = _idGenerator.GetNext()};
            }
        }

        public AtCommand CreateRequest(AtCmd atCommand, byte[] value = null)
        {
            return new AtCommand(atCommand, value) { FrameId = _idGenerator.GetNext() };
        }

        public RemoteAtCommand CreateRequest(AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null)
        {
            return new RemoteAtCommand(atCommand, remoteXbee, value) { FrameId = _idGenerator.GetNext() };
        }

        // Sending requests

        public XBeeResponse Send(byte[] payload, XBeeAddress destination = null)
        {
            return Send(CreateRequest(payload, destination));
        }

        public XBeeResponse Send(string payload, XBeeAddress destination = null)
        {
            return Send(CreateRequest(payload, destination));
        }

        public AtResponse Send(AtCmd atCommand, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtResponse)Send(CreateRequest(atCommand, value), timeout);
        }

        public RemoteAtResponse Send(AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse) Send(CreateRequest(atCommand, remoteXbee, value), timeout);
        }

        public XBeeResponse Send(XBeeRequest xbeeRequest, int timeout = PacketParser.DefaultParseTimeout)
        {
            if (xbeeRequest.FrameId == PacketIdGenerator.NoResponseId)
            {
                SendNoReply(xbeeRequest);
                return null;
            }

            var filter = xbeeRequest is AtCommand
                            ? new AtResponseFilter((AtCommand)xbeeRequest)
                            : new PacketIdFilter(xbeeRequest);

            var listener = new SinglePacketListener(filter);

            try
            {
                AddPacketListener(listener);
                SendRequest(xbeeRequest);
                return listener.GetResponse(timeout);
            }
            finally
            {
                _parser.RemovePacketListener(listener);
            }
        }

        public void SendNoReply(AtCmd atCommand, byte[] value = null)
        {
            SendNoReply(CreateRequest(atCommand, value));
        }

        public void SendNoReply(byte[] payload, XBeeAddress destination = null)
        {
            SendNoReply(CreateRequest(payload, destination));
        }

        public void SendNoReply(string payload, XBeeAddress destination = null)
        {
            SendNoReply(CreateRequest(payload, destination));
        }

        public void SendNoReply(XBeeRequest request)
        {
            // we don't expect any response to this request
            request.FrameId = PacketIdGenerator.NoResponseId;
            SendRequest(request);
        }

        public AsyncSendResult BeginSend(XBeeRequest request, IPacketListener responseListener = null)
        {
            if (responseListener == null)
                responseListener = new SinglePacketListener();

            AddPacketListener(responseListener);
            SendRequest(request);
            return new AsyncSendResult(this, responseListener);
        }

        protected void SendRequest(XBeeRequest request)
        {
            IsRequestSupported(request);

            if (_addressLookupEnabled)
                _addressLookupListener.CurrentRequest = request;

            if (Logger.IsActive(LogLevel.Debug))
                Logger.Debug("Sending " + request.GetType().Name + ": " + request);

            var bytes = request.GetXBeePacket().ToByteArray();

            if (Logger.IsActive(LogLevel.LowDebug))
                Logger.LowDebug("Sending " + ByteUtils.ToBase16(bytes));
            
            _connection.Send(bytes);
        }

        protected void IsRequestSupported(XBeeRequest request)
        {
            if (Config.IsSeries1() && request is IZigbeePacket)
                throw new ArgumentException("You are connected to a Series 1 radio but attempting to send Series 2 requests");

            if (Config.IsSeries2() && request is IWpanPacket)
                throw new ArgumentException("You are connected to a Series 2 radio but attempting to send Series 1 requests");
        }

        // Receiving responses

        public XBeeResponse[] EndReceive(AsyncSendResult asyncResult, int timeout = -1)
        {
            return asyncResult.EndReceive(timeout);
        }

        public XBeeResponse Receive(Type expectedType = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            var listener = new SinglePacketListener(new PacketTypeFilter(expectedType ?? typeof(XBeeResponse)));

            try
            {
                AddPacketListener(listener);
                return listener.GetResponse(timeout);
            }
            finally
            {
                _parser.RemovePacketListener(listener);
            }
        }

        public XBeeResponse[] CollectResponses(int timeout = -1, Type expectedPacketType = null, byte maxPacketCount = byte.MaxValue)
        {
            return CollectResponses(timeout, new PacketCountFilter(maxPacketCount, expectedPacketType ?? typeof(XBeeResponse)));
        }

        public XBeeResponse[] CollectResponses(int timeout = -1, IPacketFilter filter = null)
        {
            var listener = new PacketListener(filter);

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

        internal void NotifyDataReceived(byte[] payload, XBeeAddress sender)
        {
            if (DataReceived != null)
                DataReceived(this, payload, sender);
        }

        public event XBeeDataReceivedEventHandler DataReceived;

        public delegate void XBeeDataReceivedEventHandler(XBee receiver, byte[] data, XBeeAddress sender);
    }
}