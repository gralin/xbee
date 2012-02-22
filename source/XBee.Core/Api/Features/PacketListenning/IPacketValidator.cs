namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
{
    public interface IPacketValidator
    {
        bool Validate(XBeeResponse packet);
    }
}