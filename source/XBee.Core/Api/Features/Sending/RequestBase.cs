using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
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
        protected XBeeAddress Destination;

        protected RequestBase(XBee localXBee)
        {
            LocalXBee = localXBee;
        }

        protected void Init()
        {
            ExpectedResponse = Response.Single;
            TimeoutValue = PacketParser.DefaultParseTimeout;
            Destination = null;
            Filter = null;
        }

        #region IRequest Members

        public IRequest NoResponse()
        {
            ExpectedResponse = Response.None;
            return this;
        }

        public IRequest MultiResponse()
        {
            ExpectedResponse = Response.Multiple;
            return this;
        }

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

        public IRequest To(XBeeAddress destination)
        {
            Destination = destination;
            return this;
        }

        public IRequest ToAll()
        {
            Destination = LocalXBee.Config.IsSeries1()
                                ? XBeeAddress16.Broadcast
                                : XBeeAddress16.ZnetBroadcast;
            return this;
        }

        public AsyncSendResult Invoke()
        {
            var request = CreateRequest();

            if (ExpectedResponse == Response.None)
                request.FrameId = PacketIdGenerator.NoResponseId;

            switch (ExpectedResponse)
            {
                case Response.None:
                    SendExpectNoResponse(request);
                    return null;

                case Response.Single:
                    return SendExpectSingleResponse(request);

                case Response.Multiple:
                    return SendExpectMultipleResponse(request);

                default:
                    throw new NotImplementedException();
            }
        }

        public XBeeResponse[] GetResponses(int timeout = -1)
        {
            if (ExpectedResponse != Response.Multiple)
                throw new InvalidOperationException("ExpectedResponse value is invalid");

            return Invoke().EndReceive(timeout);
        }

        public XBeeResponse GetResponse(int timeout = -1)
        {
            if (ExpectedResponse != Response.Single)
                throw new InvalidOperationException("ExpectedResponse value is invalid");

            return Invoke().EndReceive(timeout)[0];
        }

        #endregion

        protected virtual void SendExpectNoResponse(XBeeRequest request)
        {
            LocalXBee.SendNoReply(request);
        }

        protected virtual AsyncSendResult SendExpectSingleResponse(XBeeRequest request)
        {
            InitFilter(request);
            return LocalXBee.BeginSend(request, new SinglePacketListener(Filter));
        }

        protected virtual AsyncSendResult SendExpectMultipleResponse(XBeeRequest request)
        {
            InitFilter(request);
            return LocalXBee.BeginSend(request, new PacketListener(Filter));
        }

        protected abstract XBeeRequest CreateRequest();

        protected void InitFilter(XBeeRequest request)
        {
            if (Filter == null)
            {
                Filter = request is At.AtCommand
                           ? new AtResponseFilter((At.AtCommand)request)
                           : new PacketIdFilter(request);   
            }
            else if (Filter is PacketIdFilter)
            {
                (Filter as PacketIdFilter).ExpectedPacketId = request.FrameId;   
            }
        }
    }
}