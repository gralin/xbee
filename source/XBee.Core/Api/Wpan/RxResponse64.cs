namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee. 64-bit address Receive packet.
    /// This packet is received when a remote radio transmits a TxRequest64 packet to this radio's SH + SL address.
    /// API ID: 0x80
    /// </summary>
    /// <remarks>
    /// Note: MY address must be set to 0xffff to receive this packet type.
    /// </remarks>
    public class RxResponse64 : RxResponse
    {
        public override void Parse(IPacketParser parser)
        {
            Source = parser.ParseAddress64();
            base.Parse(parser);
        }
    }
}