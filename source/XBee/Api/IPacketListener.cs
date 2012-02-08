namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketListener
    {
        void ProcessResponse(XBeeResponse response);
    }
}