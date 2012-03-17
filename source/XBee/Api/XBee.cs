using System;
using System.Collections;
using NETMF.OpenSource.XBee.Api.At;
using NETMF.OpenSource.XBee.Api.Wpan;
using NETMF.OpenSource.XBee.Api.Zigbee;
using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api
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

        private readonly AtRequest _atRequest;
        private readonly DataRequest _dataRequest;
        private readonly RawRequest _rawRequest;
        private readonly DataDelegateRequest _dataDelegateRequest;
        
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
            _atRequest = new AtRequest(this);
            _dataRequest = new DataRequest(this);
            _rawRequest = new RawRequest(this);
            _dataDelegateRequest = new DataDelegateRequest(this);
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

            if (Config.IsSeries2())
                EnableAddressLookup();
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
            var discoveryTimeout = Send(At.AtCmd.NodeDiscoverTimeout);

            // it seems that Zigbee modules have longer timeout (2 bytes)
            int timeout = discoveryTimeout.Value.Length == 1
                              ? discoveryTimeout.Value[0]
                              : UshortUtils.ToUshort(discoveryTimeout.Value);

            // ms + 1 extra second
            timeout = timeout * 100 + 1000;

            var request = CreateRequest(At.AtCmd.NodeDiscover);
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

        // New send methods idea

        public DataRequest Send2(string payload)
        {
            _dataRequest.Init(payload);
            return _dataRequest;
        }

        public DataRequest Send2(params byte[] payload)
        {
            _dataRequest.Init(payload);
            return _dataRequest;
        }

        public AtRequest Send2(At.AtCmd atCommand, params byte[] value)
        {
            _atRequest.Init((ushort) atCommand, value);
            return _atRequest;
        }

        public AtRequest Send2(Wpan.AtCmd atCommand, params byte[] value)
        {
            _atRequest.Init((ushort)atCommand, value);
            return _atRequest;
        }

        public AtRequest Send2(Zigbee.AtCmd atCommand, params byte[] value)
        {
            _atRequest.Init((ushort)atCommand, value);
            return _atRequest;
        }

        public RawRequest Send2(XBeeRequest request)
        {
            _rawRequest.Init(request);
            return _rawRequest;
        }

        public DataDelegateRequest Send2(PayloadDelegate payloadDelegate)
        {
            _dataDelegateRequest.Init(payloadDelegate);
            return _dataDelegateRequest;
        }

        public delegate byte[] PayloadDelegate();

        // Creating requests

        public XBeeRequest CreateRequest(string payload, XBeeAddress destination)
        {
            return CreateRequest(Arrays.ToByteArray(payload), destination);
        }

        public XBeeRequest CreateRequest(byte[] payload, XBeeAddress destination)
        {
            if (Config.IsSeries1())
                return new Wpan.TxRequest(destination, payload) 
                    {FrameId = _idGenerator.GetNext()};

            if (!(destination is XBeeAddress64) || destination == null)
                throw new ArgumentException("64 bit address expected", "destination");

            var serialNumber = (XBeeAddress64) destination;

            var networkAddress = AddressLookup.Contains(destination)
                ? (XBeeAddress16)AddressLookup[destination]
                : XBeeAddress16.Unknown;

            return new Zigbee.TxRequest(serialNumber, networkAddress, payload) 
                { FrameId = _idGenerator.GetNext() };
        }

        public XBeeRequest CreateRequest(string payload, NodeInfo destination)
        {
            return CreateRequest(Arrays.ToByteArray(payload), destination);
        }

        public XBeeRequest CreateRequest(byte[] payload, NodeInfo destination)
        {
            if (Config.IsSeries1())
                return new Wpan.TxRequest(destination.SerialNumber, payload) 
                    { FrameId = _idGenerator.GetNext() };

            return new Zigbee.TxRequest(destination.SerialNumber, destination.NetworkAddress, payload)
                { FrameId = _idGenerator.GetNext() };
        }

        public AtCommand CreateRequest(At.AtCmd atCommand, params byte[] value)
        {
            return new AtCommand((ushort)atCommand, value) { FrameId = _idGenerator.GetNext() };
        }

        public AtCommand CreateRequest(Wpan.AtCmd atCommand, params byte[] value)
        {
            return new AtCommand((ushort) atCommand, value) { FrameId = _idGenerator.GetNext() };
        }

        public AtCommand CreateRequest(Zigbee.AtCmd atCommand, params byte[] value)
        {
            return new AtCommand((ushort) atCommand, value) { FrameId = _idGenerator.GetNext() };
        }

        public AtCommand CreateRequest(ushort atCommand, params byte[] value)
        {
            return new AtCommand(atCommand, value) { FrameId = _idGenerator.GetNext() };
        }

        public RemoteAtCommand CreateRequest(At.AtCmd atCommand, XBeeAddress destination, params byte[] value)
        {
            return CreateRequest((ushort)atCommand, destination, value);
        }

        public RemoteAtCommand CreateRequest(Wpan.AtCmd atCommand, XBeeAddress destination, params byte[] value)
        {
            return CreateRequest((ushort)atCommand, destination, value);
        }

        public RemoteAtCommand CreateRequest(Zigbee.AtCmd atCommand, XBeeAddress destination, params byte[] value)
        {
            return CreateRequest((ushort)atCommand, destination, value);
        }

        public RemoteAtCommand CreateRequest(ushort atCommand, XBeeAddress destination, params byte[] value)
        {
            if (destination is XBeeAddress16)
                throw new ArgumentException("64 bit address expected", "destination");

            return new RemoteAtCommand(atCommand, (XBeeAddress64) destination, value) 
                { FrameId = _idGenerator.GetNext() };
        }

        public RemoteAtCommand CreateRequest(ushort atCommand, XBeeAddress64 destSerial, XBeeAddress16 destAddress, params byte[] value)
        {
            return new RemoteAtCommand(atCommand, destSerial, destAddress, value) 
                { FrameId = _idGenerator.GetNext() };
        }

        public RemoteAtCommand CreateRequest(ushort atCommand, NodeInfo destination, params byte[] value)
        {
            return new RemoteAtCommand(atCommand, destination.SerialNumber, destination.NetworkAddress, value) 
                { FrameId = _idGenerator.GetNext() };
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

        public AtResponse Send(At.AtCmd atCommand, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtResponse)Send(CreateRequest(atCommand, value), timeout);
        }

        public AtResponse Send(Wpan.AtCmd atCommand, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtResponse)Send(CreateRequest(atCommand, value), timeout);
        }

        public AtResponse Send(Zigbee.AtCmd atCommand, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtResponse)Send(CreateRequest(atCommand, value), timeout);
        }

        public RemoteAtResponse Send(At.AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse)Send(CreateRequest(atCommand, remoteXbee, value), timeout);
        }

        public RemoteAtResponse Send(Wpan.AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse) Send(CreateRequest(atCommand, remoteXbee, value), timeout);
        }

        public RemoteAtResponse Send(Zigbee.AtCmd atCommand, XBeeAddress remoteXbee, byte[] value = null, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse)Send(CreateRequest(atCommand, remoteXbee, value), timeout);
        }

        public XBeeResponse Send(XBeeRequest xbeeRequest, int timeout = PacketParser.DefaultParseTimeout)
        {
            if (xbeeRequest.FrameId == PacketIdGenerator.NoResponseId)
            {
                SendRequest(xbeeRequest);
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

        public void SendNoReply(At.AtCmd atCommand, byte[] value = null)
        {
            SendNoReply(CreateRequest(atCommand, value));
        }

        public void SendNoReply(Wpan.AtCmd atCommand, byte[] value = null)
        {
            SendNoReply(CreateRequest(atCommand, value));
        }

        public void SendNoReply(Zigbee.AtCmd atCommand, byte[] value = null)
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

            var bytes = XBeePacket.GetBytes(request);

            if (Logger.IsActive(LogLevel.LowDebug))
                Logger.LowDebug("Sending " + ByteUtils.ToBase16(bytes));
            
            _connection.Send(bytes);
        }

        protected void IsRequestSupported(XBeeRequest request)
        {
            // can be null when reading Config
            if (Config == null)
                return;

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