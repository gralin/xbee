using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents a 64-bit XBee Address
    /// </summary>
    public class XBeeAddress64 : XBeeAddress
    {
        public static readonly XBeeAddress64 BROADCAST = new XBeeAddress64(new byte[] { 0, 0, 0, 0, 0, 0, 0xff, 0xff });
        public static readonly XBeeAddress64 ZNET_COORDINATOR = new XBeeAddress64(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

        public XBeeAddress64()
            : base(new byte[8])
        {
        }

        public XBeeAddress64(byte[] address) 
            : base(address)
        {
        }

        public XBeeAddress64(int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8)
            : base(new[] { (byte) b1, (byte) b2, (byte) b3, (byte) b4, (byte) b5, (byte) b6, (byte) b7, (byte) b8 })
        {
        }

        /// <summary>
        /// Parses an 64-bit XBee address from a string representation
        /// </summary>
        /// <param name="addressStr">
        /// Must be in the format "## ## ## ## ## ## ## ##" (i.e. don't use 0x prefix)
        /// </param>
        public XBeeAddress64(string addressStr) 
            : this()
        {
            var addressParts = addressStr.Split(' ');

            if (addressParts.Length != Address.Length)
                throw new ArgumentException("Address string format is invalid");

            for (var i = 0; i < Address.Length; i++)
                Address[i] = ByteUtils.FromBase16(addressParts[i]);
        }
    }
}