using System;
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
            Logger.Initialize(Console.WriteLine, LogLevel.Info);

            Console.WriteLine("Connecting to XBee...");

            var xbee = new XBee("COM11", 9600);
            xbee.Open();

            // reading network addresses of the connected module

            var addressBytes = xbee.Send(AtCmd.NetworkAddress).GetResponsePayload();

            var xbeeAddress = xbee.Config.HardwareVersion == HardwareVersions.Series6
                        ? (XBeeAddress) new XBeeAddressIp(addressBytes)
                        : new XBeeAddress16(addressBytes);

            Console.WriteLine("XBee address: " + xbeeAddress);

            // setting Node Identifier

            var randomIdentifier = DateTime.Now.Ticks.ToString();
            Console.WriteLine("Setting node identifier to: " + randomIdentifier);
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

            Console.WriteLine("Press any key to exit...");
            Console.ReadLine();
        }
    }
}
