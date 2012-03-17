namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public static class AdcHelper
    {
        public static double ToMilliVolts(ushort adcReading)
        {
            return adcReading * 1200 / 1023.0;
        }
    }
}