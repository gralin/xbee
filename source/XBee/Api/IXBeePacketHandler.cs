namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IXBeePacketHandler
    {
        void HandlePacket(XBeeResponse response);
    }
}