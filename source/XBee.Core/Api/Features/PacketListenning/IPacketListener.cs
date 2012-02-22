namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
{
    public interface IPacketListener
    {
        void ProcessPacket(XBeeResponse packet);
    }
}