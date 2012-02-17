using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public static class UshortUtils
    {
        public static ushort ToUshort(int[] values)
        {
            if (values.Length != 2)
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

        public static int Msb(int value)
        {
            return (byte)(value >> 8);
        }

        public static int Lsb(int value)
        {
            return (byte)value;
        }
    }
}