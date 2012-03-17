namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class RawRequest : RequestBase
    {
        protected XBeeRequest Request;

        public RawRequest(XBee xbee)
            : base(xbee)
        {
        }

        internal void Init(XBeeRequest request)
        {
            Init();
            Request = request;
        }

        protected override XBeeRequest CreateRequest()
        {
            if (Request.FrameId == PacketIdGenerator.NoResponseId)
                ExpectedResponse = Response.None;

            return Request;
        }
    }
}
