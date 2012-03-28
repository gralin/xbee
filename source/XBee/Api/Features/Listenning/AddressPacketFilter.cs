namespace NETMF.OpenSource.XBee.Api
{
    public class AddressPacketFilter : IPacketFilter
    {
        public bool Accepted(XBeeResponse packet)
        {
            return packet is RemoteAtResponse
                || packet is Zigbee.RxResponse
                || packet is Zigbee.TxStatusResponse
                || packet is Zigbee.NodeIdentificationResponse;
        }

        public bool Finished()
        {
            return false;
        }
    }
}