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

            var xbeeAddress = new XBeeAddress16(xbee.Send(AtCmd.NetworkAddress).Value);
            Debug.Print("XBee address: " + xbeeAddress);

            // setting Node Identifier

            var randomIdentifier = DateTime.Now.Ticks.ToString();
            Debug.Print("Setting node identifier to: " + randomIdentifier);
            xbee.Config.SetNodeIdentifier(randomIdentifier);

            if (xbee.Config.IsSeries1())
            {
                WpanTests.Run(xbee);
            }
            else
            {
                ZigbeeTest.Run(xbee);
            }

            xbee.Close();
        }
    }
}
