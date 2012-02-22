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
        public static void Main()
        {
            var device = GetUsbDevice();

            Debug.Print("Connecting to XBee pluged to USB...");

            var connection = new UsbHostConnection(device);
            var xbee1 = new XBee(connection) {LogLevel = LogLevel.Info};
            xbee1.Open();

            Debug.Print("Connecting to XBee connected to UART...");

            var xbee2 = new XBee("COM4", 9600) {LogLevel = LogLevel.Info};
            xbee2.Open();

            // reading network addresses of the connected modules

            var xbee1Address = new XBeeAddress16(xbee1.Send(AtCmd.NetworkAddress).Value);
            var xbee2Address = new XBeeAddress16(xbee2.Send(AtCmd.NetworkAddress).Value);

            Debug.Print("XBee 1 address: " + xbee1Address);
            Debug.Print("XBee 2 address: " + xbee2Address);

            // setting Node Identifier of xbee2

            var randomIdentifier = DateTime.Now.Ticks.ToString();
            Debug.Print("Setting XBee 2 identifier to: " + randomIdentifier);
            xbee2.Config.SetNodeIdentifier(randomIdentifier);

            // example of reading configuration from remote XBee

            Debug.Print("Reading remote XBee 2 configuration...");
            var remoteConfiguration = XBeeConfiguration.Read(xbee1, xbee2Address);
            Debug.Print(remoteConfiguration.ToString());

            if (xbee1.Config.IsSeries1())
            {
                WpanTest.Run(xbee1, xbee2);
            }
            else
            {
                ZigbeeTest.Run(xbee1, xbee2);
            }

            Thread.Sleep(Timeout.Infinite);
        }

        private static USBH_Device GetUsbDevice()
        {
            Debug.Print("Waiting for USB Host device...");

            var deviceConnected = new ManualResetEvent(false);
            USBH_Device device = null;

            USBHostController.DeviceConnectedEvent +=
                d =>
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

            return device;
        }
    }
}
