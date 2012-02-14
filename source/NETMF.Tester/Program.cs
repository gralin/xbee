using System.Text;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Program Started");
            
            var xbee = new XBee("COM4", 9600) {LogLevel = LogLevel.LowDebug};
            
            // opening serial port and reading XBee harware/software info
            xbee.Open();

            // reading addresses of the connected module
            Debug.Print("Serial number: " + GetAddress64(xbee));
            Debug.Print("Network address: " + GetAddress16(xbee));

            ZBNodeDiscover[] foundNodes;

            while (true)
            {
                // discovering modules available in ZigBee network
                foundNodes = DiscoverNodes(xbee, 1);

                if (foundNodes.Length != 0)
                    break;
                
                Debug.Print("No nodes where discovered");
            }

            Debug.Print("RSSI: -" + GetRssi(xbee) + "dBi");
            Debug.Print("Found: " + foundNodes.Length + " nodes");

            // printing basic info about found modules
            for (var i = 0; i < foundNodes.Length; i++)
                Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

            foreach (var foundNode in foundNodes)
            {
                // This is not working yet i think....
                SendText(xbee, foundNode.NodeAddress64, "Hello XBee!");

                // setting digital I/O in all modules
                //SetOutput(xbee, foundNode.NodeAddress64, "D0", (int)XBeePin.Capability.DIGITAL_OUTPUT_HIGH);
            }
                
            Thread.Sleep(Timeout.Infinite);
        }

        private static ZBNodeDiscover[] DiscoverNodes(XBee xbee, int expectedNodeCount)
        {
            xbee.SendAsync(new AtCommand("ND"));

            // wait max 5s for expectedNodeCount packets
            var responses = xbee.CollectResponses(5000, expectedNodeCount);

            if (responses.Length > 0)
            {
                var foundNodes = new ZBNodeDiscover[responses.Length];

                for (var i = 0; i < responses.Length; i++)
                    foundNodes[i] = ZBNodeDiscover.Parse(responses[i]);

                return foundNodes;
            }

            return new ZBNodeDiscover[0];
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(new AtCommand("DB"));
            return response.Value[0];
        }

        private static XBeeAddress64 GetAddress64(XBee xbee)
        {
            var data = new IntArrayOutputStream();
            data.Write(xbee.Send(new AtCommand("SH")).Value);
            data.Write(xbee.Send(new AtCommand("SL")).Value);
            return new XBeeAddress64(data.GetIntArray());
        }

        private static XBeeAddress16 GetAddress16(XBee xbee)
        {
            var response = xbee.Send(new AtCommand("MY"));
            return new XBeeAddress16(response.Value);
        }

        private static bool SetOutput(XBee xbee, XBeeAddress64 node, string output, int state)
        {
            var request = new RemoteAtRequest(node, output, new[] { state });
            return xbee.Send(request).IsOk;
        }

        private static void SendText(XBee xbee, XBeeAddress64 node, string message)
        {
            var request = new ZNetTxRequest(node, Arrays.ToIntArray(Encoding.UTF8.GetBytes(message)));
            xbee.SendAsync(request);
        }
    }
}
