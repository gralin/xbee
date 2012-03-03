using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class ByteUtils
    {
        private const string Hex = "0123456789ABCDEF";

        public static string ToBase16(byte[] value)
        {
            var result = "";

            foreach (var b in value)
                result += ToBase16(b);

            return result;
        }

        public static string ToBase16(ushort b)
        {
            return b <= byte.MaxValue
                       ? new string(new[] {Hex[b >> 4], Hex[b & 0x0F]})
                       : new string(new[] {Hex[b >> 12], Hex[(b >> 8) & 0x0F], Hex[(b >> 4) & 0x00F], Hex[b & 0x0000F]});
        }

        /// <summary>
        /// Taken from here
        /// http://code.tinyclr.com/project/100/another-fast-hex-string-to-byte-conversion/
        /// </summary>
        public static byte FromBase16(string hexNumber)
        {
            var lowDigit = 0;
            var highDigit = 0;

            if (hexNumber.Length > 2)
                throw new InvalidCastException("The number to convert is too large for a byte, or not hexadecimal");

            switch (hexNumber.Length)
            {
                case 1:
                    lowDigit = hexNumber[0] - '0';
                    break;
                case 2:
                    lowDigit = hexNumber[1] - '0';
                    highDigit = hexNumber[0] - '0';
                    break;
            }

            if (lowDigit > 9) lowDigit -= 7;
            if (lowDigit > 15) lowDigit -= 32;
            
            if (lowDigit > 15) 
                throw new InvalidCastException("The number to convert is not hexadecimal");
            
            if (highDigit > 9) highDigit -= 7;
            if (highDigit > 15) highDigit -= 32;
            
            if (highDigit > 15) 
                throw new InvalidCastException("The number to convert is not hexadecimal");

            return (byte) (lowDigit + (highDigit << 4));
        }

        public static bool GetBit(byte value, int bit)
        {
            if (bit < 1 || bit > 8)
                throw new ArgumentException("Bit is out of range");

            return ((value >> (--bit)) & 0x1) == 0x1;
        }

        public static int Parse10BitAnalog(IInputStream input, int pos)
        {
            var adcMsb = input.Read("Analog " + pos + " MSB");
            var adcLsb = input.Read("Analog " + pos + " LSB");
            return Parse10BitAnalog(adcMsb, adcLsb);
        }

        public static int Parse10BitAnalog(byte msb, byte lsb)
        {
            // shift up bits 9 and 10 of the msb
            return ((msb & 0x3) << 8) + lsb;
        }

        public static string FormatByte(byte value)
        {
            return "0x" + ToBase16(value);
        }
    }
}