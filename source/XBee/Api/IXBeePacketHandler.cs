namespace NETMF.OpenSource.XBee.Api
{
    public interface IXBeePacketHandler
    {
        void HandlePacket(XBeeResponse response);
    }
}