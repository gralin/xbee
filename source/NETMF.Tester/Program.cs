using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Api;
using Microsoft.SPOT;

namespace NETMF.Tester
{
    public class Program
    {
        private static XBee _xbee;

        public static void Main()
        {
            Debug.Print("Program Started");
            _xbee = new XBee("COM4",9600);
            _xbee.Open();
            Debug.Print(_xbee.SayHello());
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
