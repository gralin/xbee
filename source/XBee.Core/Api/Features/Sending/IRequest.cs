namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IRequest
    {
        IRequest NoResponse();
        IRequest MultiResponse();
        
        IRequest Use(IPacketFilter filter);

        IRequest Timeout(int value);
        IRequest NoTimeout();

        IRequest To(XBeeAddress destination);
        IRequest ToAll();

        AsyncSendResult Invoke();

        XBeeResponse[] GetResponses(int timeout = -1);
        XBeeResponse GetResponse(int timeout = -1);
    }
}