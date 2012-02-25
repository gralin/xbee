using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    public class ZNetNodeIdentificationResponse : XBeeResponse
    {
        public enum Options
        {
            PACKET_ACKNOWLEDGED = 0x01,
            BROADCAST_PACKET = 0x02
        }

        // TODO this is repeated in NodeDiscover
        public enum DeviceTypes
        {
            COORDINATOR = 0x1,
            ROUTER = 0x2,
            END_DEVICE = 0x3
        }

        public enum SourceActions
        {
            PUSHBUTTON = 0x1,
            JOINING = 0x2
        }

        public XBeeAddress64 RemoteAddress64 { get; set; }
        public XBeeAddress16 RemoteAddress16 { get; set; }
        public Options Option { get; set; }

        // TODO Digi WTF why duplicated?? p.70
        public XBeeAddress64 RemoteAddress64_2 { get; set; }
        public XBeeAddress16 RemoteAddress16_2 { get; set; }

        public string NodeIdentifier { get; set; }
        public XBeeAddress16 ParentAddress { get; set; }
        public DeviceTypes DeviceType { get; set; }
        public SourceActions SourceAction { get; set; }
        public ushort ProfileId { get; set; }
        public ushort MfgId { get; set; }

        public override void Parse(IPacketParser parser)
        {
            RemoteAddress64 = parser.ParseAddress64();
            RemoteAddress16 = parser.ParseAddress16();

            Option = (Options) parser.Read("Option");

            // again with the addresses
            RemoteAddress64_2 = parser.ParseAddress64();
            RemoteAddress16_2 = parser.ParseAddress16();

            var nodeIdentifier = string.Empty;
            byte ch;

            // NI is terminated with 0
            while ((ch = parser.Read("Node Identifier")) != 0)
            {
                if (ch > 32 && ch < 126)
                    nodeIdentifier += (char) ch;
            }

            NodeIdentifier = nodeIdentifier;
            ParentAddress = parser.ParseAddress16();
            DeviceType = (DeviceTypes) parser.Read("Device Type");
            SourceAction = (SourceActions) parser.Read("Source Action");
            ProfileId = UshortUtils.ToUshort(parser.Read("Profile MSB"), parser.Read("Profile LSB"));
            MfgId = UshortUtils.ToUshort(parser.Read("MFG MSB"), parser.Read("MFG LSB"));
        }

        public override string ToString()
        {
            return "ZNetNodeIdentificationResponse [deviceType=" + DeviceType
                   + ", mfgId=" + MfgId 
                   + ", nodeIdentifier=" + NodeIdentifier
                   + ", option=" + Option 
                   + ", parentAddress=" + ParentAddress
                   + ", profileId=" + ProfileId 
                   + ", remoteAddress16=" + RemoteAddress16 
                   + ", remoteAddress16_2=" + RemoteAddress16_2
                   + ", remoteAddress64=" + RemoteAddress64
                   + ", remoteAddress64_2=" + RemoteAddress64_2
                   + ", sourceAction=" + SourceAction + "]" 
                   + base.ToString();
        }
    }
}