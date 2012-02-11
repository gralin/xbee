namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface ICollectTerminator
    {
        bool Stop(XBeeResponse response);
    }
}