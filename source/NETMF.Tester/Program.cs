using System;
using System.Collections;
using System.Threading;
using GHIElectronics.NETMF.USBHost;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.At;
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

            // reading network addresses of the connected modules
            
            var coordinatorNetworkAddress = GetAddress16(_coordinator);
            var routerNetworkAddress = GetAddress16(_router);

            Debug.Print("Coordinator network address: " + coordinatorNetworkAddress);
            Debug.Print("Router network address: " + routerNetworkAddress);

            // setting Node Identifier of router

            var randomIdentifier = DateTime.Now.Ticks.ToString();
            Debug.Print("Setting router node identifier to: " + randomIdentifier);
            _router.Config.SetNodeIdentifier(randomIdentifier);

            // example of using reading configuration from remote XBee

            var remoteConfiguration = XBeeConfiguration.Read(_coordinator, routerNetworkAddress);
            Debug.Print("Remote router configuration:");
            Debug.Print(remoteConfiguration.ToString());

            // discovering modules available in ZigBee network

            while (true)
            {
                var foundNodes = DiscoverNodes(_coordinator);

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

            if (!SendText(_router, _coordinator.Config.SerialNumber, "Hello coordinator"))
                Debug.Print("Failed to send message to coordinator");

            if (!SendText(_coordinator, _router.Config.SerialNumber, "Hello router"))
                Debug.Print("Failed to send message to router");

            // reading supply voltage

            var voltage1 = UshortUtils.ToUshort(_coordinator.Send(AtCmd.SupplyVoltage).Value);
            var voltage2 = UshortUtils.ToUshort(_router.Send(AtCmd.SupplyVoltage).Value);
            Debug.Print("Supply voltage of coordinator: " + (voltage1 / 1024.0).ToString("F2") + "V");
            Debug.Print("Supply voltage of router: " + (voltage2 / 1024.0).ToString("F2") + "V");

            Thread.Sleep(Timeout.Infinite);
        }

        private static ZBNodeDiscover[] DiscoverNodes(XBee xbee)
        {
            xbee.SendAsync(new AtCommand(AtCmd.NodeDiscover));

            // wait max 3s
            var responses = xbee.CollectResponses(5000, typeof(AtResponse));

            if (responses.Length > 0)
            {
                var foundNodes = new ArrayList();

                foreach (var response in responses)
                    if (((AtResponse)response).Command == AtCmd.NodeDiscover)
                        foundNodes.Add(ZBNodeDiscover.Parse(response));

                var result = new ZBNodeDiscover[foundNodes.Count];

                for (var i = 0; i < foundNodes.Count; i++)
                    result[i] = (ZBNodeDiscover) foundNodes[i];

                return result;
            }

            return new ZBNodeDiscover[0];
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(AtCmd.ReceivedSignalStrength);
            return response.Value[0];
        }

        private static XBeeAddress64 GetAddress64(XBee xbee)
        {
            var data = new OutputStream();
            data.Write(xbee.Send(AtCmd.SerialNumberHigh).Value);
            data.Write(xbee.Send(AtCmd.SerialNumberLow).Value);
            return new XBeeAddress64(data.ToArray());
        }

        private static XBeeAddress16 GetAddress16(XBee xbee)
        {
            var response = xbee.Send(AtCmd.NetworkAddress);
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
            var request = new RemoteAtCommand(output, node, new[] { state });
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
