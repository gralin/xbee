namespace NETMF.OpenSource.XBee.Api
{
    public interface IPacketFilter
    {
        bool Accepted(XBeeResponse packet);
        bool Finished();
    }
}