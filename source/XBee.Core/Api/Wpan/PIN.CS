namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    public static class Pin
    {
        public static int DigitalCount = 9;
        public static int AnalogCount = 6;

        public enum Digital : byte
        {
            D0,
            D1,
            D2,
            D3,
            D4,
            D5,
            D6,
            D7,
            D8
        }

        public enum Analog : byte
        {
            A0,
            A1,
            A2,
            A3,
            A4,
            A5
        }
    }
}