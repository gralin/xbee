using System;

namespace NETMF.OpenSource.XBee.Api
{
    public abstract class RequestBase : IRequest
    {
        protected enum Response
        {
            None,
            Single,
            Multiple
        }

        protected readonly XBee LocalXBee;
        protected IPacketFilter Filter;
        protected Response ExpectedResponse;
        protected int TimeoutValue;
        protected NodeInfo DestinationNode;
        protected XBeeAddress DestinationAddress;

        protected RequestBase(XBee localXBee)
        {
            LocalXBee = localXBee;
        }

        protected void Init()
        {
            ExpectedResponse = Response.Single;
            TimeoutValue = PacketParser.DefaultParseTimeout;
            DestinationAddress = null;
            DestinationNode = null;
            Filter = null;
        }

        #region IRequest Members

        public IRequest Use(IPacketFilter filter)
        {
            Filter = filter;
            return this;
        }

        public IRequest Timeout(int value)
        {
            TimeoutValue = value;
            return this;
        }

        public IRequest NoTimeout()
        {
            TimeoutValue = -1;
            return this;
        }

        public IRequest To(ushort networkAddress)
        {
            DestinationAddress = new XBeeAddress16(networkAddress);
            return this;
        }

        public IRequest To(ulong serialNumber)
        {
            DestinationAddress = new XBeeAddress64(serialNumber);
            return this;
        }

        public IRequest To(XBeeAddress destination)
        {
            DestinationAddress = destination;
            return this;
        }

        public IRequest To(NodeInfo destination)
        {
            DestinationNode = destination;
            return this;
        }

        public IRequest ToAll()
        {
            DestinationAddress = XBeeAddress64.Broadcast;
            return this;
        }

        public AsyncSendResult Invoke()
        {
            var request = CreateRequest();

            switch (ExpectedResponse)
            {
                case Response.None:
                    LocalXBee.SendNoReply(request);
                    return null;

                case Response.Single:
                    InitFilter(request);
                    return LocalXBee.BeginSend(request, new SinglePacketListener(Filter));

                case Response.Multiple:
                    InitFilter(request);
                    return LocalXBee.BeginSend(request, new PacketListener(Filter));

                default:
                    throw new NotImplementedException();
            }
        }

        public XBeeResponse[] GetResponses(int timeout = -1)
        {
            ExpectedResponse = Response.Multiple;
            return Invoke().EndReceive(timeout);
        }

        public XBeeResponse GetResponse(int timeout = -1)
        {
            ExpectedResponse = Response.Single;
            return Invoke().EndReceive(timeout)[0];
        }

        public void NoResponse()
        {
            ExpectedResponse = Response.None;
            Invoke();
        }

        #endregion

        protected abstract XBeeRequest CreateRequest();

        protected void InitFilter(XBeeRequest request)
        {
            if (Filter == null)
            {
                Filter = request is AtCommand
                           ? new AtResponseFilter((AtCommand)request)
                           : new PacketIdFilter(request);   
            }
            else if (Filter is PacketIdFilter)
            {
                (Filter as PacketIdFilter).ExpectedPacketId = request.FrameId;   
            }
        }
    }
}