using System;
using System.Diagnostics;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace PC.Tester
{
    static class ZigbeeTest
    {
        public static void Run(XBee xbee)
        {
            // discovering modules available in ZigBee network

            Debug.Print("Discovering nodes...");
            ZBNodeDiscover foundNode = null;

            while (true)
            {
                var foundNodes = DiscoverNodes(xbee);

                if (foundNodes.Length > 0)
                {
                    Debug.Print("Found: " + foundNodes.Length + " nodes");

                    for (var i = 0; i < foundNodes.Length; i++)
                    {
                        Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

                        if (foundNodes[i].NodeAddress64 != xbee.Config.SerialNumber)
                            foundNode = foundNodes[i];
                    }

                    break;
                }

                Debug.Print("No nodes where discovered");
            }

            // printing RSSI of connected module

            Debug.Print("RSSI: " + GetRssi(xbee) + "dBi");

            // reading supply voltage

            var voltage = UshortUtils.ToUshort(xbee.Send(AtCmd.SupplyVoltage).Value);
            var voltageVolts = (voltage*1200/1024.0) / 1000.0;
            Debug.Print("Supply voltage: " + voltageVolts.ToString("F2") + "V");

            // sending text messages

            xbee.AddPacketListener(new IncomingDataListener());

            while (true)
            {
                Console.WriteLine("Press enter to send hello world to remote XBee");
                var key = Console.ReadKey();

                if (key.Key != ConsoleKey.Enter)
                    return;

                if (!SendText(xbee, foundNode.NodeAddress64, "Hello world!"))
                    Console.WriteLine("Failed to send message!");
            }
        }

        private static ZBNodeDiscover[] DiscoverNodes(XBee xbee)
        {
            var asyncResult = xbee.BeginSend(xbee.CreateRequest(AtCmd.NodeDiscover), new NodeDiscoveryListener());

            // max discovery time is NC * 100 ms (by default is 6s)
            const int discoveryTimeout = 0x3C * 100;

            var nodes = xbee.EndReceive(asyncResult, discoveryTimeout);
            var result = new ZBNodeDiscover[nodes.Length];

            for (var i = 0; i < result.Length; i++)
                result[i] = ZBNodeDiscover.Parse(nodes[i]);

            return result;
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(AtCmd.ReceivedSignalStrength);
            return -1 * response.Value[0];
        }

        private static bool SendText(XBee xbee, XBeeAddress destination, string message)
        {
            var response = (ZNetTxStatusResponse)xbee.Send(destination, message);
            return response.DeliveryStatus == ZNetTxStatusResponse.DeliveryResult.Success;
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

            public XBeeResponse[] GetPackets(int timeout)
            {
                throw new NotSupportedException();
            }
        }
    }
}
