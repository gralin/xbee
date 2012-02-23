namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketListener
    {
        void ProcessPacket(XBeeResponse packet);
    }
}