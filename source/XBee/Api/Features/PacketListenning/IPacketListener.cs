namespace NETMF.OpenSource.XBee.Api
{
    public interface IPacketListener
    {
        bool Finished { get; }
        void ProcessPacket(XBeeResponse packet);
        XBeeResponse[] GetPackets(int timeout);
    }
}