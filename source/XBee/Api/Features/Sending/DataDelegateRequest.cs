namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class DataDelegateRequest : DataRequest
    {
        private XBee.PayloadDelegate _bytesDelegate;

        public override byte[] Payload
        {
            get { return _bytesDelegate.Invoke(); }
        }

        public DataDelegateRequest(XBee xbee) 
            : base(xbee)
        {
        }

        internal void Init(XBee.PayloadDelegate payloadDelegate)
        {
            Init();
            _bytesDelegate = payloadDelegate;
        }
    }
}