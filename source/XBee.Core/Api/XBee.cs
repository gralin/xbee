using System;
using System.Collections;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// This is an API for communicating with Digi XBee 802.15.4 and ZigBee radios
    /// </summary>
    public class XBee : IXBee
    {
        private readonly IXBeeConnection _connection;
        private readonly PacketParser _parser;
        private int _sequentialFrameId = 0xff;
        private readonly CountLimitTerminator _collectCountLimit;

        public LogLevel LogLevel
        {
            get { return Logger.LoggingLevel; }
            set { Logger.LoggingLevel = value; }
        }

        public HardwareVersion.RadioType RadioType { get; protected set; }

        public string FirmwareVersion { get; protected set; }

        protected XBee()
        {
            _parser = new PacketParser();
            _collectCountLimit = new CountLimitTerminator();
        }

        public XBee(IXBeeConnection connection) 
            : this()
        {
            _connection = connection;
            _connection.DataReceived += (data, offset, count) =>
            {
                var buffer = new byte[count];
                Array.Copy(data, offset, buffer, 0, count);
                _parser.AddBuffer(buffer);
            };
        }

        public XBee(string portName, int baudRate)
            : this(new SerialConnection(portName, baudRate))
        {
        }

        /// <summary>
        /// Perform startup checks
        /// </summary>
        private void DoStartupChecks()
        {
		    try 
            {
                var ap = Send(new AtCommand("AP"));

                if (!ap.IsOk)
                    throw new XBeeException("Attempt to query AP parameter failed");

                if (ap.Value[0] == 2)
                {
                    Logger.Info("Radio is in API mode with escape characters (AP=2)");
                }
                else
                {
                    Logger.LowDebug("XBee radio is in API mode without escape characters (AP=1)."
                        + " The radio must be configured in API mode with escape bytes "
                        + "(AP=2) for use with this library.");

                    Logger.LowDebug("Attempting to set AP to 2");
                    ap = Send(new AtCommand("AP", 2));

                    if (!ap.IsOk)
                        throw new XBeeException("Attempt to set AP=2 failed");

                    Logger.Info("Successfully set AP mode to 2. This setting will not "
                        + "persist a power cycle without the WR (write) command");
                }

                ap = Send(new AtCommand("HV"));
			
			    var radioType = HardwareVersion.Parse(ap);

                if (radioType == HardwareVersion.RadioType.UNKNOWN)
                {
                    Logger.Warn("Unknown radio type (HV): " + ap.Value[0]);
                }
                else
                {
                    Logger.Info("XBee radio is " + HardwareVersion.GetName(radioType));
                }
                
                var vr = Send(new AtCommand("VR"));
			
			    if (vr.IsOk)
			    {
			        FirmwareVersion = ByteUtils.ToBase16(vr.Value);
                    Logger.Info("Firmware version is " + FirmwareVersion);
			    }
			
			    ClearResponseQueue();
		    } 
            catch (XBeeTimeoutException) 
            {
			    throw new XBeeException("AT command timed-out while attempt to set/read in API mode. " 
                    + "The XBee radio must be in API mode (AP=2) to use with this library");
		    }
	    }

        public AtCommandResponse Send(AtCommand atCommand, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (AtCommandResponse)Send((XBeeRequest)atCommand, timeout);
        }

        public RemoteAtResponse Send(RemoteAtRequest remoteAtCommand, int timeout = PacketParser.DefaultParseTimeout)
        {
            return (RemoteAtResponse)Send((XBeeRequest)remoteAtCommand, timeout);
        }

        #region IXBee Members

        public void Open()
        {
            _parser.Start();
            _connection.Open();
            
            DoStartupChecks();
        }

        public void Close()
        {
            _parser.Stop();
            _connection.Close();
        }

        public void AddPacketListener(IPacketListener packetListener)
        {
            throw new NotImplementedException();
        }

        public void RemovePacketListener(IPacketListener packetListener)
        {
            throw new NotImplementedException();
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
            Logger.LowDebug("sending packet to XBee " + ByteUtils.ToBase16(packet));
            _connection.Send(Arrays.ToByteArray(packet));
        }

        public void SendAsync(XBeeRequest request)
        {
            if (RadioType != HardwareVersion.RadioType.UNKNOWN)
            {
                // TODO use interface to mark series type
                if (RadioType == HardwareVersion.RadioType.SERIES1 && request.GetType().Name.IndexOf("Api.Zigbee") > -1)
                    throw new ArgumentException("You are connected to a Series 1 radio but attempting to send Series 2 requests");

                if (RadioType == HardwareVersion.RadioType.SERIES2 && request.GetType().Name.IndexOf("Api.Wpan") > -1)
                    throw new ArgumentException("You are connected to a Series 2 radio but attempting to send Series 1 requests");
            }

            Logger.Debug("Sending packet: " + request);
            SendPacket(request.GetXBeePacket());
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
        /// <param name="timeout"></param>
        /// <exception cref="XBeeTimeoutException">
        /// XBeeTimeoutException thrown if no matching response is identified
        /// </exception>
        /// <returns></returns>
        public XBeeResponse Send(XBeeRequest xbeeRequest, int timeout = PacketParser.DefaultParseTimeout)
        {
            if (xbeeRequest.FrameId == XBeeRequest.NO_RESPONSE_FRAME_ID)
                throw new XBeeException("Frame Id cannot be 0 for a synchronous call -- it will always timeout as there is no response!");
            
            SendAsync(xbeeRequest);
            _parser.ParseTimeout = timeout;
            return _parser.GetPacket();
        }

        public XBeeResponse GetResponse()
        {
            return GetResponse(PacketParser.DefaultParseTimeout);
        }

        public XBeeResponse GetResponse(int timeout)
        {
            _parser.ParseTimeout = timeout;
            return _parser.GetPacket();
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

        public void ClearResponseQueue()
        {
            _parser.ClearPackets();
        }

        public XBeeResponse[] CollectResponses(int wait, int maxPacketCount)
        {
            _collectCountLimit.MaxPacketCount = maxPacketCount;
            return CollectResponses(wait, _collectCountLimit);
        }

        /// <summary>
        /// Collects responses until the timeout is reached or the CollectTerminator returns true
        /// </summary>
        /// <param name="wait"></param>
        /// <param name="terminator"></param>
        /// <returns></returns>
        public XBeeResponse[] CollectResponses(int wait, ICollectTerminator terminator = null)
        {
            var startTime = DateTime.Now;
            var responses = new Queue();

            while (true)
            {
                var elapsedTime = DateTime.Now.Subtract(startTime);
                var elapsedMilliseconds = elapsedTime.Ticks/TimeSpan.TicksPerMillisecond;
                var remainingMilliseconds = (int)(wait - elapsedMilliseconds);

                if (remainingMilliseconds <= 0)
                    break;

                try
                {
                    var response = GetResponse(remainingMilliseconds);
                    responses.Enqueue(response);

                    if (terminator != null && terminator.Stop(response))
                        break;
                }
                catch (XBeeTimeoutException)
                {
                    // failed to receive packet in this iteration
                }
            }

            if (responses.Count == 0)
                return new XBeeResponse[0];
            
            var result = new XBeeResponse[responses.Count];

            for (var i = 0; i < result.Length; i++)
                result[i] = (XBeeResponse) responses.Dequeue();

            return result;
        }

        #endregion
    }
}