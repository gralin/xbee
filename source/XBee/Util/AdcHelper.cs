namespace NETMF.OpenSource.XBee.Util
{
    public static class AdcHelper
    {
        public static double ToMilliVolts(ushort adcReading)
        {
            return adcReading * 1200 / 1023.0;
        }
    }
}