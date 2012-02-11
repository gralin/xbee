using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee. This packet is received when a remote XBee sends a ZNetTxRequest
    /// API ID: 0x90
    /// </summary>
    public class ZNetRxResponse : ZNetRxBaseResponse, INoRequestResponse
    {
        public int[] Data { get; set; }

        protected override void Parse(IPacketParser parser)
        {
            ParseAddress(parser);
            ParseOption(parser);
            Data = parser.ReadRemainingBytes();	
        }

        public override string ToString()
        {
            return base.ToString() +
                   ",data=" + ByteUtils.ToBase16(Data);
        }
    }
}