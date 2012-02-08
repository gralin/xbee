using System.Threading;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public class Program
    {
        private Gadgeteer.Modules.GHIElectronics.Api.XBee _xbee;

        public static void Main()
        {
            Debug.Print("Program Started");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
