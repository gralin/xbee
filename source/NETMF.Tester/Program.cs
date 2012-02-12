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
            
            var xbee = new XBee("COM4", 9600) {LogLevel = LogLevel.Info};
            xbee.Open();

            var foundNodes = DiscoverNodes(xbee, 1);
            var foundNode = foundNodes.Length > 0 ? foundNodes[0] : null;

            if (foundNode != null)
            {
                GetRssi(xbee); 
                SetOutput(xbee, foundNode, "D0", (int)XBeePin.Capability.DIGITAL_OUTPUT_HIGH);
            }

            Thread.Sleep(Timeout.Infinite);
        }

        private static ZBNodeDiscover[] DiscoverNodes(XBee xbee, int expectedNodeCount)
        {
            xbee.SendAsynchronous(new AtCommand("ND"));

            // wait max 5s for expectedNodeCount packets
            var responses = xbee.CollectResponses(5000, expectedNodeCount);

            if (responses.Length == 0)
            {
                Debug.Print("No nodes where discovered");
            }
            else
            {
                Debug.Print("Found: " + responses.Length + " nodes");

                var foundNodes = new ZBNodeDiscover[responses.Length];

                for (var i = 0; i < responses.Length; i++)
                {
                    foundNodes[i] = ZBNodeDiscover.Parse(responses[i]);
                    Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);
                }

                return foundNodes;
            }

            return new ZBNodeDiscover[0];
        }

        private static void GetRssi(XBee xbee)
        {
            var request = (AtCommandResponse) xbee.SendSynchronous(new AtCommand("DB"));
            var rssi = request.Value[0];
            Debug.Print("RSSI: -" + rssi + "dBi");
        }

        private static void SetOutput(XBee xbee, ZBNodeDiscover foundNode, string output, int state)
        {
            var request = new RemoteAtRequest(foundNode.NodeAddress64, output, new[] { state });
            var response = (RemoteAtResponse)xbee.SendSynchronous(request);

            Debug.Print(response.IsOk
                            ? "Successfully set output " + output
                            : "Attempt to set " + output + " failed. Status: " + response.ResponseStatus);
        }
    }
}
