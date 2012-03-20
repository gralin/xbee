using System;
using System.Diagnostics;
using NETMF.OpenSource.XBee.Api;
using NETMF.OpenSource.XBee.Api.Zigbee;
using NETMF.OpenSource.XBee.Util;

namespace PC.Tester
{
    static class ZigbeeTest
    {
        public static void Run(XBee xbee)
        {
            // discovering modules available in ZigBee network

            Debug.Print("Discovering nodes...");
            NodeDiscover foundNode = null;

            while (true)
            {
                var foundNodes = xbee.DiscoverNodes();

                if (foundNodes.Length == 0)
                {
                    Debug.Print("No nodes where discovered");
                    continue;
                }

                Debug.Print("Found: " + foundNodes.Length + " nodes");

                for (var i = 0; i < foundNodes.Length; i++)
                {
                    Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

                    if (foundNodes[i].SerialNumber != xbee.Config.SerialNumber)
                        foundNode = (NodeDiscover)foundNodes[i];
                }

                if (foundNode != null)
                    break;
            }

            // printing RSSI of connected module

            Debug.Print("RSSI: " + GetRssi(xbee) + "dBi");

            // reading supply voltage

            var voltage = UshortUtils.ToUshort(xbee.Send2(AtCmd.SupplyVoltage).GetResponsePayload());
            var voltageVolts = (voltage*1200/1024.0) / 1000.0;
            Debug.Print("Supply voltage: " + voltageVolts.ToString("F2") + "V");

            // sending text messages

            xbee.DataReceived += (r,d,s) => 
                Debug.Print("Received '" + Arrays.ToString(d) + "' from " + s);

            while (true)
            {
                Console.WriteLine("Press enter to send hello world to remote XBee");
                var key = Console.ReadKey();

                if (key.Key != ConsoleKey.Enter)
                    return;

                if (!SendText(xbee, foundNode.SerialNumber, "Hello world!"))
                    Console.WriteLine("Failed to send message!");
            }
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send2(AtCmd.ReceivedSignalStrength).GetResponse();
            return -1 * response.Value[0];
        }

        private static bool SendText(XBee xbee, XBeeAddress destination, string message)
        {
            var response = (TxStatusResponse)xbee.Send2(message).To(destination).GetResponse();
            return response.DeliveryStatus == TxStatusResponse.DeliveryResult.Success;
        }
    }
}
