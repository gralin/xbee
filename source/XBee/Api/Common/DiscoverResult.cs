namespace NETMF.OpenSource.XBee.Api.Common
{
    public abstract class DiscoverResult
    {
        public NodeInfo NodeInfo { get; set; }

        public override string ToString()
        {
            return "address=" + NodeInfo.NetworkAddress
                   + ", serial=" + NodeInfo.SerialNumber
                   + ", id=" + NodeInfo.NodeIdentifier;
        }
    }
}