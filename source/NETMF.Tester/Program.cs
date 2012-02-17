using System.Text;
using System.Threading;
using GHIElectronics.NETMF.USBHost;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.Zigbee;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public class Program
    {
        private static XBee _coordinator;
        private static XBee _router;

        public static void Main()
        {
            Debug.Print("Waiting for USB Host device...");

            var deviceConnected = new ManualResetEvent(false);
            USBH_Device device = null;

            USBHostController.DeviceConnectedEvent += d =>
            {
                device = d;
                deviceConnected.Set();
            };

            if (USBHostController.GetDevices().Length > 0)
            {
                device = USBHostController.GetDevices()[0];
            }
            else
            {
                deviceConnected.WaitOne();
            }

            Debug.Print("Connecting to coordinator...");

            var connection = new UsbHostConnection(device);
            _coordinator = new XBee(connection) { LogLevel = LogLevel.Info };
            _coordinator.Open();

            Debug.Print("Connecting to router...");

            _router = new XBee("COM4", 9600) { LogLevel = LogLevel.Info };
            _router.Open();
            
            // reading addresses of the connected modules
            
            var coordinator64 = GetAddress64(_coordinator);
            var coordinator16 = GetAddress16(_coordinator);
            Debug.Print("Coordinator serial number: " + coordinator64);
            Debug.Print("Coordinator network address: " + coordinator16);
            
            var router64 = GetAddress64(_router);
            var router16 = GetAddress16(_router);
            Debug.Print("Router serial number: " + router64);
            Debug.Print("Router network address: " + router16);

            // discovering modules available in ZigBee network

            const int expectedNodesCount = 1;

            while (true)
            {
                var foundNodes = DiscoverNodes(_coordinator, expectedNodesCount);

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

            Debug.Print("Coordinator RSSI: -" + GetRssi(_coordinator) + "dBi");
            Debug.Print("Router RSSI: -" + GetRssi(_router) + "dBi");

            // sending text messages

            _coordinator.AddPacketListener(new IncomingDataListener());
            _router.AddPacketListener(new IncomingDataListener());

            if (!SendText(_router, coordinator64, "Hello coordinator"))
                Debug.Print("Failed to send message to coordinator");

            if (!SendText(_coordinator, router64, "Hello router"))
                Debug.Print("Failed to send message to router");

            Thread.Sleep(Timeout.Infinite);
        }

        private static ZBNodeDiscover[] DiscoverNodes(XBee xbee, int expectedNodeCount)
        {
            xbee.SendAsync(new AtCommand("ND"));

            // wait max 5s for expectedNodeCount packets
            var responses = xbee.CollectResponses(5000, typeof(AtCommandResponse), expectedNodeCount);

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

        private static bool SendText(XBee xbee, XBeeAddress64 dest64, string message)
        {
            var request = new ZNetTxRequest(dest64, message);
            var response = (ZNetTxStatusResponse)xbee.Send(request, typeof(ZNetTxStatusResponse));
            return response.DeliveryStatus == ZNetTxStatusResponse.DeliveryResult.SUCCESS;
        }

        private static bool SetOutput(XBee xbee, XBeeAddress64 node, string output, int state)
        {
            var request = new RemoteAtRequest(node, output, new[] { state });
            return xbee.Send(request).IsOk;
        }
    }

    public class IncomingDataListener : IPacketListener
    {
        public void ProcessPacket(XBeeResponse response)
        {
            var dataPacket = response as ZNetRxResponse;
            
            if (dataPacket == null) 
                return;
            
            Debug.Print("Received '" + Arrays.ToString(dataPacket.Data) 
                + "' from " + dataPacket.RemoteAddress16);
        }
    }
}
