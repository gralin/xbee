using System;
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

            Console.WriteLine("Discovering nodes...");
            NodeDiscover foundNode = null;

            while (true)
            {
                var foundNodes = xbee.DiscoverNodes();

                if (foundNodes.Length == 0)
                {
                    Console.WriteLine("No nodes where discovered");
                    continue;
                }

                Console.WriteLine("Found: " + foundNodes.Length + " nodes");

                for (var i = 0; i < foundNodes.Length; i++)
                {
                    Console.WriteLine("#" + (i + 1) + " - " + foundNodes[i]);

                    if (foundNodes[i].SerialNumber != xbee.Config.SerialNumber)
                        foundNode = (NodeDiscover)foundNodes[i];
                }

                if (foundNode != null)
                    break;
            }

            // printing RSSI of connected module

            Console.WriteLine("RSSI: " + GetRssi(xbee) + "dBi");

            // reading supply voltage

            var voltage = UshortUtils.ToUshort(xbee.Send(AtCmd.SupplyVoltage).GetResponsePayload());
            var voltageVolts = (voltage*1200/1024.0) / 1000.0;
            Console.WriteLine("Supply voltage: " + voltageVolts.ToString("F2") + "V");

            // sending text messages

            xbee.DataReceived += (r,d,s) =>
                Console.WriteLine("Received '" + Arrays.ToString(d) + "' from " + s);

            while (true)
            {
                Console.WriteLine("Press enter to send hello world to remote XBee");
                var key = Console.ReadKey();

                if (key.Key != ConsoleKey.Enter)
                    return;

                if (!SendText(xbee, new XBeeAddress64(0x0013A2004086AD25), "Hello world!"))
                    Console.WriteLine("Failed to send message!");
            }
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(AtCmd.ReceivedSignalStrength).GetResponse();
            return -1 * response.Value[0];
        }

        private static bool SendText(XBee xbee, XBeeAddress destination, string message)
        {
            var response = (TxStatusResponse)xbee.Send(message).To(destination).GetResponse();
            return response.DeliveryStatus == TxStatusResponse.DeliveryResult.Success;
        }
    }
}
