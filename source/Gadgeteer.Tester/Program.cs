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
            Debug.Print("Program Started");
        }
    }
}
