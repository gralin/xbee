using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Api;
using Gadgeteer.Modules.GHIElectronics.Util;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public class Program
    {
        private static XBee _xbee;

        public static void Main()
        {
            Debug.Print("Program Started");
            _xbee = new XBee("COM4", 9600) {LogLevel = LogLevel.Info};
            _xbee.Open();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
