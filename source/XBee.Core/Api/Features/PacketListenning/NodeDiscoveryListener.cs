using Gadgeteer.Modules.GHIElectronics.Api.At;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class NodeDiscoveryListener : MultiplePacketListener
    {
        public NodeDiscoveryListener(int maxNodes = int.MaxValue)
            : base(maxNodes, typeof(object))
        {
            Validator = new AtCommandValidator(AtCmd.NodeDiscover);
            Terminator = new CountLimitTerminator(maxNodes);
        }
    }
}