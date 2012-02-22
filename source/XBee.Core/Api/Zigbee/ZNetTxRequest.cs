using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee.  Sends a packet to a remote radio.
    /// The remote radio receives the data as a ZNetRxResponse packet.
    /// API ID: 0x10
    /// </summary>
    public class ZNetTxRequest : XBeeRequest, IZigbeePacket
    {
	    public enum Options
        {
		    UNICAST = 0,
		    BROADCAST = 8
	    }

        // 10/28/08 the datasheet states 72 is maximum payload size but I was able to push 75 through successfully, 
        // even with all bytes as escape bytes (a total post-escape packet size of 169!).

        /**
         * This is the maximum payload size for ZNet firmware, as specified in the datasheet.
         * This value is provided for reference only and is not enforced by this software unless
         * max size unless specified in the setMaxPayloadSize(int) method.
         * Note: this size refers to the packet size prior to escaping the control bytes.
         * Note: ZB Pro firmware provides the ATNP command to determine max payload size.
         * For ZB Pro firmware, the TX Status will return a PAYLOAD_TOO_LARGE (0x74) delivery status 
         * if the payload size is exceeded
         */

        public const int ZNET_MAX_PAYLOAD_SIZE = 72;
        public const int DEFAULT_BROADCAST_RADIUS = 0;

        public XBeeAddress64 DestinationSerial { get; set; }
        public XBeeAddress16 DestinationAddress { get; set; }
        public int BroadcastRadius { get; set; }
        public Options Option { get; set; }
        public int[] Payload { get; set; }
        public int MaxPayloadSize { get; set; }

        /// <summary>
        /// From manual p. 33:
        /// The ZigBee Transmit Request API frame specifies the 64-bit Address and the network address (if
        /// known) that the packet should be sent to. By supplying both addresses, the module will forego
        /// network address Discovery and immediately attempt to route the data packet to the remote. If the
        /// network address of a particular remote changes, network address and route discovery will take
        /// place to establish a new route to the correct node. Upon successful
        /// </summary>
        /// <remarks>
        /// Key points:
        /// <list type="bullet">
        ///    <item>always specify the 64-bit address and also specify the 16-bit address, if known. 
        ///          Set the 16-bit network address to 0xfffe if not known.</item>
        ///    <item>check the 16-bit address of the tx status response frame as it may change.</item>
        ///    <item>keep a hash table mapping of 64-bit address to 16-bit network address.</item>
        /// </list>
        /// </remarks>
        /// <param name="destination"></param>
        /// <param name="payload"></param>
        /// <param name="broadcastRadius"></param>
        /// <param name="option"></param>
        /// <param name="frameId"></param>
        public ZNetTxRequest(XBeeAddress destination, int[] payload, int broadcastRadius = DEFAULT_BROADCAST_RADIUS, 
            Options option = Options.UNICAST, int frameId = DEFAULT_FRAME_ID)
        {
            if (destination is XBeeAddress16)
            {
                DestinationAddress = (XBeeAddress16)destination;
                DestinationSerial = XBeeAddress64.BROADCAST;
            }
            else
            {
                DestinationAddress = XBeeAddress16.BROADCAST;
                DestinationSerial = (XBeeAddress64)destination;
            }

            FrameId = frameId;
            BroadcastRadius = broadcastRadius;
            Option = option;
            Payload = payload;
        }

        protected OutputStream GetFrameDataAsIntArrayOutputStream()
        {
            if (MaxPayloadSize > 0 && Payload.Length > MaxPayloadSize)
                throw new ArgumentException("Payload exceeds user-defined maximum payload size of " 
                    + MaxPayloadSize + " bytes.  Please package into multiple packets");

            var output = new OutputStream();
        
            // api id
		    output.Write((byte) ApiId); 
		
		    // frame id (arbitrary byte that will be sent back with ack)
            output.Write(FrameId);
		
		    // add 64-bit dest address
            output.Write(DestinationSerial.Address);
		
		    // add 16-bit dest address
            output.Write(DestinationAddress.Address);
		
		    // write broadcast radius
            output.Write(BroadcastRadius);
		
		    // write options byte
            output.Write((byte) Option);

            output.Write(Payload);

            return output;
        }

        public override ApiId ApiId
        {
            get { return ApiId.ZNET_TX_REQUEST; }
        }

        public override int[] GetFrameData()
        {
            return GetFrameDataAsIntArrayOutputStream().ToArray();
        }

        public override string ToString()
        {
            return base.ToString() +
                ",destAddr64=" + DestinationSerial +
                ",destAddr16=" + DestinationAddress +
                ",broadcastRadius=" + BroadcastRadius +
                ",option=" + Option +
                ",payload=int[" + Payload.Length + "]";
        }
    }
}
