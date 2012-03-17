using System;

namespace NETMF.OpenSource.XBee.Util
{
    public static class UshortUtils
    {
        public static ushort ToUshort(byte[] values)
        {
            if (values.Length < 2)
                throw new ArgumentException();

            return ToUshort(values[0], values[1]);
        }

        public static ushort ToUshort(int msb, int lsb)
        {
            return ToUshort((byte) msb, (byte) lsb);
        }

        public static ushort ToUshort(byte msb, byte lsb)
        {
            return (ushort) ((msb << 8) + lsb);
        }

        public static ushort FromAscii(string value)
        {
            if (value.Length < 2)
                throw new ArgumentException("Value lenght should be 2");

            return ToUshort(value[0], value[1]);
        }

        public static byte Msb(ushort value)
        {
            return (byte)(value >> 8);
        }

        public static byte Lsb(ushort value)
        {
            return (byte)value;
        }

        public static string ToAscii(ushort value)
        {
            return new string(new[] { (char)Msb(value), (char)Lsb(value) });
        }

        public static ushort Parse10BitAnalog(byte[] value)
        {
            if (value.Length != 2)
                throw new ArgumentOutOfRangeException("value");

            return Parse10BitAnalog(value[0], value[1]);
        }

        public static ushort Parse10BitAnalog(byte msb, byte lsb)
        {
            // shift up bits 9 and 10 of the msb
            return (ushort)(((msb & 0x3) << 8) + lsb);
        }

        /// <summary>
        /// Get the value of specified <paramref name="bit"/>.
        /// </summary>
        /// <param name="value">Value to get the <paramref name="bit"/> value from</param>
        /// <param name="bit">Number of bit to retrieve (0 is LSB)</param>
        /// <returns><c>true</c> if bit is set, <c>false</c> otherwise</returns>
        public static bool GetBit(ushort value, byte bit)
        {
            if (bit > 15)
                throw new ArgumentOutOfRangeException("bit");

            return ((value >> bit) & 0x1) == 0x1;
        }
    }
}