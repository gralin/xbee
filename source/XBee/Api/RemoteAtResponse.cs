namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Supported by both series 1 (10C8 firmware and later) and series 2.
    /// Represents a response, corresponding to a RemoteAtRequest.
    /// API ID: 0x97
    /// </summary>
    public class RemoteAtResponse : AtCommandResponse
    {
        public XBeeAddress64 RemoteAddress64 { get; set; }
        public XBeeAddress16 RemoteAddress16 { get; set; }

        public bool IsSixteenBitAddressUnknown
        {
            get { return RemoteAddress16.Address.Msb == 0xFF 
                      && RemoteAddress16.Address.Lsb == 0xFE; }
        }

        public override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("Remote AT Response Frame Id");
            RemoteAddress64 = parser.ParseAddress64();
            RemoteAddress16 = parser.ParseAddress16();
            Char1 = parser.Read("Command char 1");
            Char2 = parser.Read("Command char 2");
            ResponseStatus = (Status) parser.Read("AT Response Status");
            Value = parser.ReadRemainingBytes();
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",remoteAddress64=" + RemoteAddress64
                   + ",remoteAddress16=" + RemoteAddress16;
        }
    }
}