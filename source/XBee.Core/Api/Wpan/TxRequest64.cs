namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    public class TxRequest64 : TxRequest
    {
        public new XBeeAddress64 Destination
        {
            get { return (XBeeAddress64)base.Destination; }
            set { base.Destination = value; }
        }

        /// <summary>
        /// 16 bit Tx Request.
        /// </summary>
        /// <remarks>
        /// Note: if option is DISABLE_ACK_OPTION you will not get a ack response and you must use the asynchronous send method.
        /// Payload size is limited to 100 bytes, according to MaxStream documentation.
        /// Keep in mind that if you programmed the destination address with X-CTU, the unit is hex, so if you set MY=1234, use 0x1234.
        /// </remarks>
        /// <param name="destination"></param>
        /// <param name="payload"></param>
        /// <param name="frameId"></param>
        /// <param name="option"></param>
        public TxRequest64(XBeeAddress64 destination, int[] payload, int frameId = DEFAULT_FRAME_ID, Options option = Options.Unicast)
        {
            Destination = destination;
            Payload = payload;
            FrameId = frameId;
            Option = option;
        }

        public override ApiId ApiId
        {
            get { return ApiId.TX_REQUEST_64; }
        }
    }
}