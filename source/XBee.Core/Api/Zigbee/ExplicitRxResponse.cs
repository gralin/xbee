using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee. This packet is received when a remote XBee sends a ExplicitTxRequest
    /// Radio must be configured for explicit frames to use this class (AO=1)
    /// </summary>
    public class ExplicitRxResponse : RxResponse
    {
        public byte SourceEndpoint { get; set; }
        public byte DestinationEndpoint { get; set; }
        public ushort ClusterId { get; set; }
        public ushort ProfileId { get; set; }

        public override void Parse(IPacketParser parser)
        {
            SourceSerial = parser.ParseAddress64();
            SourceAddress = parser.ParseAddress16();
            SourceEndpoint = parser.Read("Reading Source Endpoint");
            DestinationEndpoint = parser.Read("Reading Destination Endpoint");
            ClusterId = UshortUtils.ToUshort(parser.Read("Reading Cluster Id MSB"), parser.Read("Reading Cluster Id LSB"));
            ProfileId = UshortUtils.ToUshort(parser.Read("Reading Profile Id MSB"), parser.Read("Reading Profile Id LSB"));
            Option = (Options)parser.Read("ZNet RX Option");
            Payload = parser.ReadRemainingBytes();
        }

        public override string ToString()
        {
            return base.ToString() +
                   ",sourceEndpoint=" + ByteUtils.ToBase16(SourceEndpoint) +
                   ",destinationEndpoint=" + ByteUtils.ToBase16(DestinationEndpoint) +
                   ",clusterId=" + ByteUtils.ToBase16(ClusterId) +
                   ",profileId=" + ByteUtils.ToBase16(ProfileId);
        }
    }
}