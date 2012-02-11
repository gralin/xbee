namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IResponseFilter
    {
        bool Accept(XBeeResponse response);
    }
}