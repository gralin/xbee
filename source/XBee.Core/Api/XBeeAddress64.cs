using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents a 64-bit XBee Address
    /// </summary>
    public class XBeeAddress64 : XBeeAddress
    {
        public static readonly XBeeAddress64 BROADCAST = new XBeeAddress64(new[] { 0, 0, 0, 0, 0, 0, 0xff, 0xff });
        public static readonly XBeeAddress64 ZNET_COORDINATOR = new XBeeAddress64(new[] { 0, 0, 0, 0, 0, 0, 0, 0 });

        private int[] _address;

        /// <summary>
        /// Parses an 64-bit XBee address from a string representation
        /// </summary>
        /// <param name="addressStr">
        /// Must be in the format "## ## ## ## ## ## ## ##" (i.e. don't use 0x prefix)
        /// </param>
        public XBeeAddress64(string addressStr)
        {
            _address = new int[8];

            var addressParts = addressStr.Split(' ');

            if (addressParts.Length != _address.Length)
                throw new ArgumentException("Address string format is invalid");

            for (var i = 0; i < _address.Length; i++)
                _address[i] = ByteUtils.FromBase16(addressParts[i]);
        }

        public XBeeAddress64(int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8)
        {
            _address = new[] { b1, b2, b3, b4, b5, b6, b7, b8 };
        }

        public XBeeAddress64(int[] address)
        {
            _address = address;
        }

        public XBeeAddress64()
        {
            _address = new int[8];
        }

        public void SetAddress(int[] address)
        {
            _address = address;
        }

        public bool Equals(XBeeAddress64 other)
        {
            return Arrays.AreEqual(_address, other._address);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (XBeeAddress64)) return false;
            return Equals((XBeeAddress64) obj);
        }

        public override int GetHashCode()
        {
            return _address != null ? Arrays.HashCode(_address) : 0;
        }

        public override int[] GetAddress()
        {
            return _address;
        }

        public override string ToString()
        {
            return ByteUtils.ToBase16(_address);
        }
    }
}