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
            if (Destination != null)
                throw new System.InvalidOperationException("Destination needs to be set directly in request");

            if (Request.FrameId == PacketIdGenerator.NoResponseId)
            {
                ExpectedResponse = Response.None;
                return Request;
            }

            return Request;
        }
    }
}
