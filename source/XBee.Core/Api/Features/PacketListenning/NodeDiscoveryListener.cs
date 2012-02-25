using Gadgeteer.Modules.GHIElectronics.Api.At;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class NodeDiscoveryListener : MultiplePacketListener
    {
        public NodeDiscoveryListener(int maxNodes = int.MaxValue)
            : base(maxNodes, null)
        {
            Validator = new AtCommandValidator(AtCmd.NodeDiscover);
        }
    }
}