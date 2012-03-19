using System;
using System.Diagnostics;
using NETMF.OpenSource.XBee.Api;
using NETMF.OpenSource.XBee.Api.Common;
using NETMF.OpenSource.XBee.Util;
using LogLevel = NETMF.OpenSource.XBee.Util.LogLevel;

namespace PC.Tester
{
    class Program
    {
        static void Main()
        {
            Logger.Initialize(m => Debug.Print(m), LogLevel.Info);

            Debug.Print("Connecting to XBee...");

            var xbee = new XBee("COM11", 9600);
            xbee.Open();

            // reading network addresses of the connected module

            var addressBytes = xbee.Send2(AtCmd.NetworkAddress).GetResponsePayload();

            var xbeeAddress = xbee.Config.HardwareVersion == HardwareVersions.Series6
                        ? (XBeeAddress) new XBeeAddressIp(addressBytes)
                        : new XBeeAddress16(addressBytes);

            Debug.Print("XBee address: " + xbeeAddress);

            // setting Node Identifier

            var randomIdentifier = DateTime.Now.Ticks.ToString();
            Debug.Print("Setting node identifier to: " + randomIdentifier);
            xbee.Config.SetNodeIdentifier(randomIdentifier);

            if (xbee.Config.IsSeries1())
            {
                WpanTests.Run(xbee);
            }
            else if (xbee.Config.IsSeries2())
            {
                ZigbeeTest.Run(xbee);
            }

            xbee.Close();

            Debug.Print("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
