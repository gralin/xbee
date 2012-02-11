using System;
using System.Collections;
using System.IO;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// This is an API for communicating with Digi XBee 802.15.4 and ZigBee radios
    /// </summary>
    public class XBee : IXBee
    {
        private readonly IXBeeConnection _connection;

        public HardwareVersion.RadioType RadioType { get; protected set; }

        public XBee(IXBeeConnection connection)
        {
            _connection = connection;
            _connection.DataReceived += OnDataReceived;
        }

        public XBee(string portName, int baudRate)
        {
            _connection = new SerialConnection(portName, baudRate);
            _connection.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(byte[] data, int offset, int count)
        {
            // TODO: handle the received data
        }

        /// <summary>
        /// Perform startup checks
        /// </summary>
        private void DoStartupChecks()
        {
		    try 
            {				
			    var ap = SendAtCommand(new AtCommand("AP"));

			    if (!ap.IsOk)
				    throw new XBeeException("Attempt to query AP parameter failed");

                if (ap.Value[0] == 2)
                {
                    Debug.Print("Radio is in correct AP mode (AP=2)");
                }
                else
                {
                    Debug.Print("XBee radio is in API mode without escape characters (AP=1)."
                        + " The radio must be configured in API mode with escape bytes " 
                        + "(AP=2) for use with this library.");

                    Debug.Print("Attempting to set AP to 2");
                    ap = SendAtCommand(new AtCommand("AP", 2));

                    if (!ap.IsOk)
                        throw new XBeeException("Attempt to set AP=2 failed");

                    Debug.Print("Successfully set AP mode to 2. This setting will not " 
                        + "persist a power cycle without the WR (write) command");
                }

                ap = SendAtCommand(new AtCommand("HV"));
			
			    var radioType = HardwareVersion.Parse(ap);

                Debug.Print("XBee radio is " + radioType);

                if (radioType == HardwareVersion.RadioType.UNKNOWN)
                    Debug.Print("Unknown radio type (HV): " + ap.Value[0]);
			
			    var vr = SendAtCommand(new AtCommand("VR"));
			
			    if (vr.IsOk)
                    Debug.Print("Firmware version is " + ByteUtils.ToBase16(vr.Value));
			
			    ClearResponseQueue();
		    } 
            catch (XBeeTimeoutException) 
            {
			    throw new XBeeException("AT command timed-out while attempt to set/read in API mode. " 
                    + "The XBee radio must be in API mode (AP=2) to use with this library");
		    }
	    }

        /// <summary>
        /// Uses sendSynchronous to send an AtCommand and collect the response
        /// </summary>
        /// <remarks>
        /// Timeout value is fixed at 5 seconds
        /// </remarks>
        /// <param name="command"></param>
        /// <returns></returns>
        private AtCommandResponse SendAtCommand(AtCommand command)
        {
            return (AtCommandResponse)SendSynchronous(command, 5000);
        }

        public void SendRequest(XBeeRequest request)
        {
            if (RadioType != HardwareVersion.RadioType.UNKNOWN)
            {
                // TODO use interface to mark series type
                if (RadioType == HardwareVersion.RadioType.SERIES1 && request.GetType().Name.IndexOf("Api.Zigbee") > -1)
                    throw new ArgumentException("You are connected to a Series 1 radio but attempting to send Series 2 requests");
 
                if (RadioType == HardwareVersion.RadioType.SERIES2 && request.GetType().Name.IndexOf("Api.Wpan") > -1)
                    throw new ArgumentException("You are connected to a Series 2 radio but attempting to send Series 1 requests");
            }

            Debug.Print("Sending request to XBee: " + request);
            SendPacket(request.GetXBeePacket());
        }

        #region IXBee Members

        public void Open()
        {
            _connection.Open();
            
            //DoStartupChecks();
        }

        public void Close()
        {
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
            throw new NotImplementedException();
        }

        public void SendAsynchronous(XBeeRequest xbeeRequest)
        {
            throw new NotImplementedException();
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
        public XBeeResponse SendSynchronous(XBeeRequest xbeeRequest, int timeout)
        {
            throw new NotImplementedException();
        }

        public XBeeResponse GetResponse()
        {
            throw new NotImplementedException();
        }

        public XBeeResponse GetResponse(int timeout)
        {
            throw new NotImplementedException();
        }

        public int GetCurrentFrameId()
        {
            throw new NotImplementedException();
        }

        public int GetNextFrameId()
        {
            throw new NotImplementedException();
        }

        public void UpdateFrameId(int val)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void ClearResponseQueue()
        {
            throw new NotImplementedException();
        }

        public XBeeResponse[] CollectResponses(int wait, ICollectTerminator terminator)
        {
            throw new NotImplementedException();
        }

        #endregion

        public string SayHello()
        {
            return "Hello from XBee";
        }
    }
}