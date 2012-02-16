using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class DoubleByte
    {
        public int Msb { get; set; }
        public int Lsb { get; set; }

        public DoubleByte()
        {
        }

        /// <summary>
        /// Constructs a 16bit value from two bytes (high and low)
        /// </summary>
        /// <param name="msb"></param>
        /// <param name="lsb"></param>
        public DoubleByte(int msb, int lsb)
        {
            if (msb > 0xFF || lsb > 0xFF)
                throw new ArgumentOutOfRangeException("msb or lsb are out of range");

            Msb = msb;
            Lsb = lsb;
        }

        /// <summary>
        /// Decomposes a 16bit int into high and low bytes
        /// </summary>
        /// <param name="val"></param>
        public DoubleByte(int val)
        {
            if (val > 0xFFFF || val < 0)
                throw new ArgumentOutOfRangeException("value is out of range");

            // split address into high and low bytes
            Msb = val >> 8;
            Lsb = val & 0xFF;
        }

        /// <summary>
        /// Constructs a 16bit value from byte array
        /// </summary>
        /// <param name="val"></param>
        public DoubleByte(int[] array)
        {
            if (array.Length != 2 || array[0] > 0xFF || array[1] > 0xFF)
                throw new ArgumentOutOfRangeException("value is out of range");

            // split address into high and low bytes
            Msb = array[0];
            Lsb = array[1];
        }

        public int Get16BitValue()
        {
            return (Msb << 8) + Lsb;
        }

        public int[] GetArray()
        {
            return new[] { Msb, Lsb };
        }

        public bool Equals(DoubleByte other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return other.Msb == Msb && other.Lsb == Lsb;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (DoubleByte)) return false;
            return Equals((DoubleByte) obj);
        }

        public override int GetHashCode()
        {
            var result = Msb;
            result = 31 * result + Lsb;
            return result;
        }
    }
}