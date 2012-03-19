namespace NETMF.OpenSource.XBee.Api
{
    public class AtRequest : RequestBase
    {
        protected ushort AtCommand;
        protected byte[] Value;

        public AtRequest(XBee localXBee) 
            : base(localXBee)
        {
        }

        internal void Init(ushort atCommand, params byte[] value)
        {
            Init();
            AtCommand = atCommand;
            Value = value;
        }

        protected override XBeeRequest CreateRequest()
        {
            // if address other than null or local XBee serial number was provided
            // the AT command will be sent to remote node

            if (DestinationNode != null)
                return LocalXBee.CreateRequest(AtCommand, DestinationNode, Value);

            return DestinationAddress == null
                ? LocalXBee.CreateRequest(AtCommand, Value)
                : LocalXBee.CreateRequest(AtCommand, DestinationAddress, Value);
        }

        public new AtResponse GetResponse(int timeout = -1)
        {
            return (AtResponse) base.GetResponse(timeout);
        }

        public byte[] GetResponsePayload(int timeout = -1)
        {
            return GetResponse(timeout).Value;
        }
    }
}
