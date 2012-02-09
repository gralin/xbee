using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Api;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Program Started");
            var xbee = new XBee();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
