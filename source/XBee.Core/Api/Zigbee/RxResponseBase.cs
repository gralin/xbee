namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    public abstract class RxResponseBase : XBeeResponse
    {
        public enum Options
        {
            PacketAcknowledged = 0x01,
            BroadcastPacket = 0x02
        }

        public XBeeAddress64 SourceSerial { get; set; }
        public XBeeAddress16 SourceAddress { get; set; }
        public Options Option { get; set; }

        public override void Parse(IPacketParser parser)
        {
            SourceSerial = parser.ParseAddress64();
            SourceAddress = parser.ParseAddress16();
            Option = (Options)parser.Read("ZNet RX Option");
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",sourceSerial=" + SourceSerial
                   + ",sourceAddress=" + SourceAddress
                   + ",option=" + Option;
        }
    }
}