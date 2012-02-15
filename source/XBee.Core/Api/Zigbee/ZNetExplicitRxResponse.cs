using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee. This packet is received when a remote XBee sends a ZNetExplicitTxRequest
    /// Radio must be configured for explicit frames to use this class (AO=1)
    /// </summary>
    public class ZNetExplicitRxResponse : ZNetRxResponse
    {
        public int SourceEndpoint { get; set; }
        public int DestinationEndpoint { get; set; }
        public DoubleByte ClusterId { get; set; }
        public DoubleByte ProfileId { get; set; }

        public override void Parse(IPacketParser parser)
        {
            ParseAddress(parser);
            SourceEndpoint = parser.Read("Reading Source Endpoint");
            DestinationEndpoint = parser.Read("Reading Destination Endpoint");
            ClusterId = new DoubleByte(parser.Read("Reading Cluster Id MSB"), parser.Read("Reading Cluster Id LSB"));
            ProfileId = new DoubleByte(parser.Read("Reading Profile Id MSB"), parser.Read("Reading Profile Id LSB"));
            ParseOption(parser);
            Data = parser.ReadRemainingBytes();
        }

        public override string ToString()
        {
            return base.ToString() +
                ",sourceEndpoint=" + ByteUtils.ToBase16(SourceEndpoint) +
                ",destinationEndpoint=" + ByteUtils.ToBase16(DestinationEndpoint) +
                ",clusterId(msb)=" + ByteUtils.ToBase16(ClusterId.Msb) +
                ",clusterId(lsb)=" + ByteUtils.ToBase16(ClusterId.Lsb) +
                ",profileId(msb)=" + ByteUtils.ToBase16(ProfileId.Msb) +
                ",profileId(lsb)=" + ByteUtils.ToBase16(ProfileId.Lsb);
        }
    }
}