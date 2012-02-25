namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee.  
    /// Common elements of 16 and 64 bit Address Receive packets.
    /// </summary>
    public abstract class RxResponse : XBeeResponse
    {
        public enum Options
        {
            None = 0,
            AddressBroadcast = 2,
            PanBroadcast = 4
        }

        public int Rssi { get; set; }
        public Options Option { get; set; }
        public XBeeAddress Source { get; set; }
        public byte[] Payload { get; set; }

        public override void Parse(IPacketParser parser)
        {
            Source = parser.ApiId == ApiId.RX_16_RESPONSE
                         ? (XBeeAddress) parser.ParseAddress16()
                         : parser.ParseAddress64();

            Rssi = -1 * parser.Read("RSSI");
            Option = (Options) parser.Read("Options");
            Payload = parser.ReadRemainingBytes();
        }
        
        public string GetOptionName(Options option)
        {
            switch (option)
            {
                case Options.None: 
                    return "None";
                case Options.AddressBroadcast:
                    return "AddressBroadcast";
                case Options.PanBroadcast: 
                    return "PanBroadcast";
                default:
                    return string.Empty;
            }
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",rssi=" + Rssi + "dBi"
                   + ",option=" + GetOptionName(Option)
                   + ",source=" + Source
                   + ",payload=byte[" + Payload.Length + "]";
        }
    }
}