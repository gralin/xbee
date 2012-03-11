using Gadgeteer.Modules.GHIElectronics.Api;
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

            coordinator.DataReceived += OnDataReceived;
            router.DataReceived += OnDataReceived;

            router.Send("Hello coordinator", coordinator.Config.SerialNumber);
            coordinator.Send("Hello router", router.Config.SerialNumber);

            // reading supply voltage

            var voltage1 = UshortUtils.ToUshort(coordinator.Send(AtCmd.SupplyVoltage).Value);
            var voltage2 = UshortUtils.ToUshort(router.Send(AtCmd.SupplyVoltage).Value);
            var voltage1Volts = AdcHelper.ToMilliVolts(voltage1) / 1000.0;
            var voltage2Volts = AdcHelper.ToMilliVolts(voltage1) / 1000.0;

            Debug.Print("Supply voltage of coordinator: " + voltage1Volts.ToString("F2") + "V");
            Debug.Print("Supply voltage of router: " + voltage2Volts.ToString("F2") + "V");
        }

        private static int GetRssi(XBee xbee)
        {
            var response = xbee.Send(AtCmd.ReceivedSignalStrength);
            return -1 * response.Value[0];
        }

        private static void OnDataReceived(XBee receiver, byte[] data, XBeeAddress sender)
        {
            Debug.Print(receiver.Config.SerialNumber + " <- '" + Arrays.ToString(data) + "' from " + sender);
        }
    }
}