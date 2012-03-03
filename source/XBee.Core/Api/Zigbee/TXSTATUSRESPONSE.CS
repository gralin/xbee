namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee.  This is sent out the UART of the transmitting XBee immediately following
    /// a Transmit packet.  Indicates if the Transmit packet (TxRequest) was successful.
    /// </summary>
    public class TxStatusResponse : XBeeFrameIdResponse
    {
        public enum DeliveryResult
        {
            Success = 0,
            CcaFailure = 0x02,
            InvalidDestinationEndpoint = 0x15,
            NetworkAckFailure = 0x21,
            NotJoinedToNetwork = 0x22,
            SelfAddressed = 0x23,
            AddressNotFound = 0x24,
            RouteNotFound = 0x25,
            /// <summary>
            /// ZB Pro firmware only
            /// </summary>
            PayloadTooLarge = 0x74
        }

        public enum DiscoveryResult
        {
            NoDiscovery = 0,
            AddressDiscovery = 1,
            RouteDiscovery = 2,
            AddressAndRouteDiscovery = 3
        }

        public XBeeAddress16 DestinationAddress { get; set; }
        public byte RetryCount { get; set; }
        public DeliveryResult DeliveryStatus { get; set; }
        public DiscoveryResult DiscoveryStatus { get; set; }

        /// <summary>
        /// Returns true if the delivery status is SUCCESS
        /// </summary>
        public bool IsSuccess { get { return DeliveryStatus == DeliveryResult.Success; } }

        public override void Parse(IPacketParser parser)
        {
            base.Parse(parser);
            DestinationAddress = parser.ParseAddress16();
            RetryCount = parser.Read("ZNet Tx Status Tx Count");
            DeliveryStatus = (DeliveryResult) parser.Read("ZNet Tx Status Delivery Status");
            DiscoveryStatus = (DiscoveryResult) parser.Read("ZNet Tx Status Discovery Status");
        }

        public override string ToString()
        {
            return base.ToString() +
            ",destinationAddress=" + DestinationAddress +
            ",retryCount=" + RetryCount +
            ",deliveryStatus=" + DeliveryStatus +
            ",discoveryStatus=" + DiscoveryStatus;
        }
    }
}