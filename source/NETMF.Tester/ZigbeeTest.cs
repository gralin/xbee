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
    public static class ZigbeeTest
    {
        public static void Run(XBee coordinator, XBee router)
        {
            // discovering modules available in ZigBee network

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

            Debug.Print("Coordinator RSSI: -" + GetRssi(coordinator) + "dBi");
            Debug.Print("Router RSSI: -" + GetRssi(router) + "dBi");

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
                    result[i] = (ZBNodeDiscover)foundNodes[i];

                return result;
            }

            return new ZBNodeDiscover[0];
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(AtCmd.ReceivedSignalStrength);
            return response.Value[0];
        }

        private static bool SendText(XBee xbee, XBeeAddress64 dest64, string message)
        {
            var request = new ZNetTxRequest(dest64, message);
            var response = (ZNetTxStatusResponse)xbee.Send(request, typeof(ZNetTxStatusResponse));
            return response.DeliveryStatus == ZNetTxStatusResponse.DeliveryResult.SUCCESS;
        }

        class IncomingDataListener : IPacketListener
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
}