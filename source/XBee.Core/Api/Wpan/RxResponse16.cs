namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee. 
    /// 16-bit address Receive packet.
    /// This packet is received when a remote radio transmits a TxRequest16 packet to this radio's MY address.
    /// API ID: 0x81
    /// </summary>
    public class RxResponse16 : RxResponse
    {
        public override void Parse(IPacketParser parser)
        {
            Source = parser.ParseAddress16();
            base.Parse(parser);
        }
    }
}