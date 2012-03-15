using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class DataRequest : RequestBase
    {
        public virtual byte[] Payload { get; set; }

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
            if (DestinationNode != null) 
               return LocalXBee.CreateRequest(Payload, DestinationNode);

            if (DestinationAddress == null)
                DestinationAddress = XBeeAddress64.Broadcast;

            return LocalXBee.CreateRequest(Payload, DestinationAddress);
        }
    }
}
