using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class DataRequest : RequestBase
    {
        protected byte[] Payload;

        public DataRequest(XBee xbee)
            : base(xbee)
        {
        }

        internal void Init(byte[] payload)
        {
            Init();
            Payload = payload;
        }

        internal void Init(string payload)
        {
            Init();
            Payload = Arrays.ToByteArray(payload);
        }

        protected override XBeeRequest CreateRequest()
        {
            if (Destination == null)
                throw new System.ArgumentException("Destination needs to be specified");

            return LocalXBee.CreateRequest(Payload, Destination);
        }
    }
}
