using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents a 16-bit XBee Address.
    /// </summary>
    public class XBeeAddress16 : XBeeAddress
    {
        public static readonly XBeeAddress16 BROADCAST = new XBeeAddress16(0xFF, 0xFF);
        public static readonly XBeeAddress16 ZNET_BROADCAST = new XBeeAddress16(0xFF, 0xFE);

        public DoubleByte Address { get; set; }

        public XBeeAddress16()
        {
            Address = new DoubleByte();
        }

        private XBeeAddress16(int msb, int lsb)
        {
            Address = new DoubleByte(msb, lsb);
        }

        public XBeeAddress16(int[] arr)
        {
            Address = new DoubleByte(arr);
        }

        public bool Equals(XBeeAddress16 other)
        {
            return Address != null ? Address.Equals(other.Address) : other.Address == null;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (XBeeAddress16)) return false;
            return Equals((XBeeAddress16) obj);
        }

        public override int GetHashCode()
        {
            return Address != null ? Address.GetHashCode() : 0;
        }

        public override int[] GetAddress()
        {
            return Address.GetArray();
        }
    }
}