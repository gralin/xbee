namespace NETMF.OpenSource.XBee.Api
{
    public class NodeDiscoveryFilter : AtResponseFilter
    {
        private bool _finished;

        public NodeDiscoveryFilter(int packetId)
            : base((ushort) At.AtCmd.NodeDiscover, packetId)
        {
        }

        public override bool Accepted(XBeeResponse packet)
        {
            if (!base.Accepted(packet))
                return false;

            var atResponse = (At.AtResponse)packet;

            // empty response is received in series 1 modules
            // in series 2 the timeout determines the end of discovery
            if (atResponse.Value == null || atResponse.Value.Length == 0)
            {
                _finished = true;
                return false;
            }

            return true;
        }

        public override bool Finished()
        {
            return _finished;
        }
    }
}