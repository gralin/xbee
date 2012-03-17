using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class DataReceivedListener : IPacketListener
    {
        private readonly XBee _xbee;

        public DataReceivedListener(XBee xbee)
        {
            _xbee = xbee;
        }

        public bool Finished
        {
            get { return false; }
        }

        public void ProcessPacket(XBeeResponse packet)
        {
            if (packet is Wpan.RxResponse)
            {
                var response = packet as Wpan.RxResponse;
                _xbee.NotifyDataReceived(response.Payload, response.Source);
            }
            else if (packet is Zigbee.RxResponse)
            {
                var response = packet as Zigbee.RxResponse;
                _xbee.NotifyDataReceived(response.Payload, response.SourceSerial);
            }
        }

        public XBeeResponse[] GetPackets(int timeout)
        {
            throw new NotSupportedException();
        }
    }
}