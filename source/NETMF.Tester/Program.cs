using System.Threading;
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

            DiscoverNodes(xbee, 1);

            Thread.Sleep(Timeout.Infinite);
        }

        private static void DiscoverNodes(IXBee xbee, int expectedNodeCount)
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

                for (var i = 0; i < responses.Length; i++)
                    Debug.Print("#" + (i + 1) + " - " + ZBNodeDiscover.Parse(responses[i]));
            }
        }
    }
}
