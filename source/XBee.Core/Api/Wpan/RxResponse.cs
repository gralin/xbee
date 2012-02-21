using System;

namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee.  
    /// Common elements of 16 and 64 bit Address Receive packets.
    /// </summary>
    public abstract class RxResponse : XBeeResponse
    {
        [Flags]
        public enum Options
        {
            None = 0,
            AddressBroadcast = 2,
            PanBroadcast = 4
        }

        public int Rssi { get; set; }
        public Options Option { get; set; }
        public XBeeAddress Source { get; set; }
        public int[] Payload { get; set; }

        public override void Parse(IPacketParser parser)
        {
            Rssi = -1 * parser.Read("RSSI");
            Option = (Options) parser.Read("Options");
            Payload = parser.ReadRemainingBytes();
        }
        
        public override string ToString()
        {
            return base.ToString()
                   + ",rssi=" + Rssi
                   + ",options=" + Option
                   + ",source=" + Source
                   + ",payload=" + Payload;
        }
    }
}