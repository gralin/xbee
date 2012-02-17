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
            Debug.Print("Router serial number: " + GetAddress64(_router));
            Debug.Print("Router network address: " + GetAddress16(_router));

            ZBNodeDiscover[] foundNodes;

            while (true)
            {
                // discovering modules available in ZigBee network
                foundNodes = DiscoverNodes(_coordinator, 1);

                if (foundNodes.Length != 0)
                    break;
                
                Debug.Print("No nodes where discovered");
            }

            Debug.Print("Coordinator RSSI: -" + GetRssi(_coordinator) + "dBi");
            Debug.Print("Router RSSI: -" + GetRssi(_router) + "dBi");

            Debug.Print("Found: " + foundNodes.Length + " nodes");

            // printing basic info about found modules
            for (var i = 0; i < foundNodes.Length; i++)
                Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

            SendText(_router, coordinator64, coordinator16, "Hello world!");
            Debug.Print(ReceiveText(_coordinator));

            //foreach (var node in foundNodes)
            //{
            //    try
            //    {
            //        Debug.Print(SendText(_coordinator, new XBeeAddress64("01 02 03 04 05 06 07 08"), new XBeeAddress16(new[]{1,2}),  "XXX")
            //                        ? "Success!"
            //                        : "Failed!");
            //    }
            //    catch (XBeeTimeoutException)
            //    {
            //        Debug.Print("Failed to send text packet - timeout");
            //    }

            //    // setting digital I/O in all modules
            //    //SetOutput(xbee, foundNode.NodeAddress64, "D0", (int)XBeePin.Capability.DIGITAL_OUTPUT_HIGH);
            //}
                
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

        private static bool SetOutput(XBee xbee, XBeeAddress64 node, string output, int state)
        {
            var request = new RemoteAtRequest(node, output, new[] { state });
            return xbee.Send(request).IsOk;
        }

        private static bool SendText(XBee xbee, XBeeAddress64 dest64, XBeeAddress16 dest16, string message)
        {
            var request = new ZNetTxRequest(dest64, dest16, Arrays.ToIntArray(Encoding.UTF8.GetBytes(message)));
            var response = (ZNetTxStatusResponse)xbee.Send(request, typeof(ZNetTxStatusResponse));
            return response.DeliveryStatus == ZNetTxStatusResponse.DeliveryResult.SUCCESS;
        }

        private static string ReceiveText(XBee xbee)
        {
            var response = (ZNetRxResponse)xbee.Receive(typeof(ZNetRxResponse));
            return new string(Encoding.UTF8.GetChars(Arrays.ToByteArray(response.Data)));
        }
    }
}
