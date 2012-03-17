namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IRequest
    {
        IRequest Use(IPacketFilter filter);

        IRequest Timeout(int value);
        IRequest NoTimeout();

        IRequest To(ushort networkAddress);
        IRequest To(ulong serialNumber);
        IRequest To(XBeeAddress destination);
        IRequest To(NodeInfo destination);
        IRequest ToAll();

        AsyncSendResult Invoke();

        void NoResponse();
        XBeeResponse[] GetResponses(int timeout = -1);
        XBeeResponse GetResponse(int timeout = -1);
    }
}