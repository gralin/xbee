namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Add an implementation of this interface to XBee.AddPacketListener 
    /// to get notifications of new packets
    /// </summary>
    public interface IPacketListener
    {
        void ProcessPacket(XBeeResponse response);
    }
}