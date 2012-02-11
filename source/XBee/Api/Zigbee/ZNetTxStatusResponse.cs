namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee.  This is sent out the UART of the transmitting XBee immediately following
    /// a Transmit packet.  Indicates if the Transmit packet (ZNetTxRequest) was successful.
    /// </summary>
    public class ZNetTxStatusResponse : XBeeFrameIdResponse
    {
        public enum DeliveryResult
        {
            SUCCESS = 0,
            CCA_FAILURE = 0x02,
            INVALID_DESTINATION_ENDPOINT = 0x15,
            NETWORK_ACK_FAILURE = 0x21,
            NOT_JOINED_TO_NETWORK = 0x22,
            SELF_ADDRESSED = 0x23,
            ADDRESS_NOT_FOUND = 0x24,
            ROUTE_NOT_FOUND = 0x25,
            /// <summary>
            /// ZB Pro firmware only
            /// </summary>
            PAYLOAD_TOO_LARGE = 0x74
        }

        public enum DiscoveryResult
        {
            NO_DISCOVERY = 0,
            ADDRESS_DISCOVERY = 1,
            ROUTE_DISCOVERY = 2,
            ADDRESS_AND_ROUTE_DISCOVERY = 3
        }

        public XBeeAddress16 RemoteAddress16 { get; set; }
        public int RetryCount { get; set; }
        public DeliveryResult DeliveryStatus { get; set; }
        public DiscoveryResult DiscoveryStatus { get; set; }

        /// <summary>
        /// Returns true if the delivery status is SUCCESS
        /// </summary>
        public bool IsSuccess { get { return DeliveryStatus == DeliveryResult.SUCCESS; } }

        public override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("ZNet Tx Status Frame Id");
            RemoteAddress16 = parser.ParseAddress16();
            RetryCount = parser.Read("ZNet Tx Status Tx Count");
            DeliveryStatus = (DeliveryResult) parser.Read("ZNet Tx Status Delivery Status");
            DiscoveryStatus = (DiscoveryResult) parser.Read("ZNet Tx Status Discovery Status");
        }

        public override string ToString()
        {
            return base.ToString() +
            ",remoteAddress16=" + RemoteAddress16 +
            ",retryCount=" + RetryCount +
            ",deliveryStatus=" + DeliveryStatus +
            ",discoveryStatus=" + DiscoveryStatus;
        }
    }
}