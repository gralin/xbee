using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public static class UshortUtils
    {
        public static ushort ToUshort(int[] values)
        {
            if (values.Length < 2)
                throw new ArgumentException();

            return ToUshort(values[0], values[1]);
        }

        public static ushort ToUshort(int msb, int lsb)
        {
            return ToUshort((byte) msb, (byte) lsb);
        }

        public static ushort ToUshort(byte msb, int lsb)
        {
            return (ushort) ((msb << 8) + lsb);
        }

        public static ushort FromAscii(string value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Value lenght should be 2");

            return ToUshort(value[0], value[1]);
        }

        public static int Msb(int value)
        {
            return (byte)(value >> 8);
        }

        public static int Lsb(int value)
        {
            return (byte)value;
        }

        public static string ToAscii(ushort value)
        {
            return new string(new[] { (char)Lsb(value), (char)Msb(value)});
        }
    }
}