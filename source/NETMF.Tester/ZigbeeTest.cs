using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public static class ZigbeeTest
    {
        public static void Run(XBee coordinator, XBee router)
        {
            // discovering modules available in ZigBee network
            
            Debug.Print("Discovering nodes...");

            while (true)
            {
                var foundNodes = DiscoverNodes(coordinator);

                if (foundNodes.Length > 0)
                {
                    Debug.Print("Found: " + foundNodes.Length + " nodes");

                    for (var i = 0; i < foundNodes.Length; i++)
                        Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

                    break;
                }

                Debug.Print("No nodes where discovered");
            }

            // printing RSSI of connected modules

            Debug.Print("Coordinator RSSI: " + GetRssi(coordinator) + "dBi");
            Debug.Print("Router RSSI: " + GetRssi(router) + "dBi");

            // sending text messages

            coordinator.AddPacketListener(new IncomingDataListener());
            router.AddPacketListener(new IncomingDataListener());

            if (!SendText(router, coordinator.Config.SerialNumber, "Hello coordinator"))
                Debug.Print("Failed to send message to coordinator");

            if (!SendText(coordinator, router.Config.SerialNumber, "Hello router"))
                Debug.Print("Failed to send message to router");

            // reading supply voltage

            var voltage1 = UshortUtils.ToUshort(coordinator.Send(AtCmd.SupplyVoltage).Value);
            var voltage2 = UshortUtils.ToUshort(router.Send(AtCmd.SupplyVoltage).Value);
            Debug.Print("Supply voltage of coordinator: " + (voltage1 / 1024.0).ToString("F2") + "V");
            Debug.Print("Supply voltage of router: " + (voltage2 / 1024.0).ToString("F2") + "V");
        }

        private static ZBNodeDiscover[] DiscoverNodes(XBee xbee)
        {
            var listener = new NodeDiscoveryListener(1);

            try
            {
                xbee.AddPacketListener(listener);
                xbee.SendAsync(AtCmd.NodeDiscover);
                var nodes = listener.GetPackets(5000);

                var result = new ZBNodeDiscover[nodes.Length];
                for (var i = 0; i < result.Length; i++)
                    result[i] = ZBNodeDiscover.Parse(nodes[i]);
                return result;
            }
            finally
            {
                xbee.RemovePacketListener(listener);
            }
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(AtCmd.ReceivedSignalStrength);
            return -1 * response.Value[0];
        }

        private static bool SendText(XBee xbee, XBeeAddress destination, string message)
        {
            var response = (ZNetTxStatusResponse)xbee.Send(destination, message);
            return response.DeliveryStatus == ZNetTxStatusResponse.DeliveryResult.SUCCESS;
        }

        class IncomingDataListener : IPacketListener
        {
            public bool Finished
            {
                get { return false; }
            }

            public void ProcessPacket(XBeeResponse packet)
            {
                if (!(packet is ZNetRxResponse))
                    return;

                var dataPacket = packet as ZNetRxResponse;

                Debug.Print("Received '" + Arrays.ToString(dataPacket.Payload)
                    + "' from " + dataPacket.SourceAddress);
            }
        }
    }
}