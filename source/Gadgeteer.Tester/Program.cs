using System.Text;
using Gadgeteer.Modules.GHIElectronics.Api;
using Microsoft.SPOT;

namespace Gadgeteer.Tester
{
    public partial class Program
    {
        // This method is run when the mainboard is powered up or reset.   
        void ProgramStarted()
        {
            xbee.Configure(9600, Interfaces.Serial.SerialParity.None, Interfaces.Serial.SerialStopBits.One, 8);

            Debug.Print("XBee config: " + xbee.Api.Config);

            xbee.Api.DataReceived += OnDataReceived;
            xbee.Api.Send("Hello World!");
        }

        private static void OnDataReceived(XBee receiver, byte[] data, XBeeAddress sender)
        {
            var dataAsString = new string(Encoding.UTF8.GetChars(data));
            Debug.Print(receiver.Config.SerialNumber + " <- " + dataAsString + " from " + sender);
        }
    }
}
