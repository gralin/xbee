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

        public XBeeAddress64() 
            : base(new int[8])
        {
        }

        public XBeeAddress64(int[] address) 
            : base(address)
        {
        }

        public XBeeAddress64(int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8)
            : base(new[] { b1, b2, b3, b4, b5, b6, b7, b8 })
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