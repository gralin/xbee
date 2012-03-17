namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketFilter
    {
        bool Accepted(XBeeResponse packet);
        bool Finished();
    }
}