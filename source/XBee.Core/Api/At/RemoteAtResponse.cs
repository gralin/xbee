using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// Supported by both series 1 (10C8 firmware and later) and series 2.
    /// Represents a response, corresponding to a RemoteAtCommand.
    /// API ID: 0x97
    /// </summary>
    public class RemoteAtResponse : AtResponse
    {
        public XBeeAddress64 RemoteAddress64 { get; set; }
        public XBeeAddress16 RemoteAddress16 { get; set; }

        public bool IsSixteenBitAddressUnknown
        {
            get { return RemoteAddress16 == XBeeAddress16.ZnetBroadcast; }
        }

        public override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("Frame Id");

            RemoteAddress64 = parser.ParseAddress64();
            RemoteAddress16 = parser.ParseAddress16();

            Command = (AtCmd)UshortUtils.ToUshort(
                parser.Read("AT Response Char 1"),
                parser.Read("AT Response Char 2"));

            Status = (AtResponseStatus) parser.Read("AT Response Status");
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