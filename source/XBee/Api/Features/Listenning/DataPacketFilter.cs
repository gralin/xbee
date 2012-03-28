namespace NETMF.OpenSource.XBee.Api
{
    public class DataPacketFilter : IPacketFilter
    {
        public bool Accepted(XBeeResponse packet)
        {
            return packet is Wpan.RxResponse 
                || packet is Zigbee.RxResponse;
        }

        public bool Finished()
        {
            return false;
        }
    }
}