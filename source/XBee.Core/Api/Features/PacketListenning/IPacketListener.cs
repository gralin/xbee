namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketListener
    {
        bool Finished { get; }
        void ProcessPacket(XBeeResponse packet);
    }
}