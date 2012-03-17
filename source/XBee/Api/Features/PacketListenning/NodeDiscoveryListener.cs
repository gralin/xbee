namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class NodeDiscoveryListener : PacketListener
    {
        public NodeDiscoveryListener(byte packetId = PacketIdGenerator.DefaultId)
            : base(new NodeDiscoveryFilter(packetId))
        {
        }
    }
}