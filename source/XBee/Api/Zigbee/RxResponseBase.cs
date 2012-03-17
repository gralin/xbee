using System;

namespace NETMF.OpenSource.XBee.Api.Zigbee
{
    public abstract class RxResponseBase : XBeeResponse
    {
        [Flags]
        public enum Options
        {
            Acknowledged = 0x01,
            Broadcast = 0x02,
            Encrypted,
            FromEndDevice = 0x40
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
                   + ",options=" + Option;
        }
    }
}