namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketValidator
    {
        bool Validate(XBeeResponse packet);
    }
}