using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents a 16-bit XBee Address.
    /// </summary>
    public class XBeeAddress16 : XBeeAddress
    {
        public static readonly XBeeAddress16 BROADCAST = new XBeeAddress16(new[] { 0xFF, 0xFF });
        public static readonly XBeeAddress16 ZNET_BROADCAST = new XBeeAddress16(new[] { 0xFF, 0xFE });

        public new ushort Address { get { return UshortUtils.ToUshort(base.Address); } }

        public XBeeAddress16() : base(new int[2])
        {
        }

        public XBeeAddress16(int[] address)
            : base(address)
        {
        }

        public XBeeAddress16(int msb, int lsb)
            : base(new[] { msb, lsb })
        {
        }

        public XBeeAddress16(ushort address) 
            : base(Arrays.ToIntArray(address))
        {
        }
    }
}