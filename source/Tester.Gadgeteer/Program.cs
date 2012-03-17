using System.Text;
using Microsoft.SPOT;
using NETMF.OpenSource.XBee.Api;

namespace Gadgeteer.Tester
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            try
            {
                Debug.Print("XBee config: " + xbee.Api.Config);
                xbee.Api.DataReceived += OnDataReceived;
                xbee.Api.Send("Hello World!");
            }
            catch (XBeeTimeoutException)
            {
                Debug.Print("XBee is not responding");
            }
        }

        private static void OnDataReceived(XBee receiver, byte[] data, XBeeAddress sender)
        {
            var dataAsString = new string(Encoding.UTF8.GetChars(data));
            Debug.Print(receiver.Config.SerialNumber + " <- " + dataAsString + " from " + sender);
        }
    }
}
