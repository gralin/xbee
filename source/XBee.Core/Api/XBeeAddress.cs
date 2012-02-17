using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public abstract class XBeeAddress
    {
        public int[] Address { get; protected set; }

        public int this[int index]
        {
            get { return Address[index]; }
            set { Address[index] = value; }
        }

        protected XBeeAddress(int[] address)
        {
            Address = address;
        }

        public override string ToString()
        {
            return ByteUtils.ToBase16(Address);
        }

        public bool Equals(XBeeAddress other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Arrays.AreEqual(other.Address, Address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (XBeeAddress)) return false;
            return Equals((XBeeAddress) obj);
        }

        public override int GetHashCode()
        {
            return (Address != null ? Address.GetHashCode() : 0);
        }
    }
}