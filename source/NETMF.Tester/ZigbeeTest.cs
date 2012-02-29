using System;
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
            
            Debug.Print("Discovering from coordinator...");

            var foundNodes = coordinator.DiscoverNodes();

            Debug.Print("Found: " + foundNodes.Length + " nodes");

            for (var i = 0; i < foundNodes.Length; i++)
                Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

            Debug.Print("Discovering from router...");

            foundNodes = router.DiscoverNodes();

            Debug.Print("Found: " + foundNodes.Length + " nodes");

            for (var i = 0; i < foundNodes.Length; i++)
                Debug.Print("#" + (i + 1) + " - " + foundNodes[i]);

            // printing RSSI of connected modules

            Debug.Print("Coordinator RSSI: " + GetRssi(coordinator) + "dBi");
            Debug.Print("Router RSSI: " + GetRssi(router) + "dBi");

            // sending text messages

            coordinator.AddPacketListener(new IncomingDataListener(coordinator.Config.SerialNumber));
            router.AddPacketListener(new IncomingDataListener(router.Config.SerialNumber));

            if (!SendText(router, coordinator.Config.SerialNumber, "Hello coordinator"))
                Debug.Print("Failed to send message to coordinator");

            if (!SendText(coordinator, router.Config.SerialNumber, "Hello router"))
                Debug.Print("Failed to send message to router");

            // reading supply voltage

            var voltage1 = UshortUtils.ToUshort(coordinator.Send(AtCmd.SupplyVoltage).Value);
            var voltage2 = UshortUtils.ToUshort(router.Send(AtCmd.SupplyVoltage).Value);
            var voltage1Volts = (voltage1 * 1200 / 1024.0) / 1000.0;
            var voltage2Volts = (voltage2 * 1200 / 1024.0) / 1000.0;

            Debug.Print("Supply voltage of coordinator: " + voltage1Volts.ToString("F2") + "V");
            Debug.Print("Supply voltage of router: " + voltage2Volts.ToString("F2") + "V");
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
            private readonly XBeeAddress _receiver;

            public bool Finished
            {
                get { return false; }
            }

            public IncomingDataListener(XBeeAddress receiver)
            {
                _receiver = receiver;
            }

            public void ProcessPacket(XBeeResponse packet)
            {
                if (!(packet is ZNetRxResponse))
                    return;

                var dataPacket = packet as ZNetRxResponse;

                Debug.Print(_receiver + " <- '" + Arrays.ToString(dataPacket.Payload)
                    + "' from " + dataPacket.SourceAddress);
            }

            public XBeeResponse[] GetPackets(int timeout)
            {
                throw new NotSupportedException();
            }
        }
    }
}