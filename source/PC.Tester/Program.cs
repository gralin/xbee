using System;
using System.Diagnostics;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Util;
using LogLevel = Gadgeteer.Modules.GHIElectronics.Util.LogLevel;

namespace PC.Tester
{
    class Program
    {
        static void Main()
        {
            Logger.Initialize(m => Debug.Print(m), LogLevel.Info);

            Debug.Print("Connecting to XBee...");

            var xbee = new XBee("COM12", 9600);
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
