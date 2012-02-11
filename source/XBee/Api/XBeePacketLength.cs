using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Supports a 16-bit XBee packet length
    /// </summary>
    public class XBeePacketLength : DoubleByte
    {
        /// <summary>
        /// Manual says max packet length is 100 bytes so not sure why 2 bytes are needed
        /// </summary>
        /// <param name="msb"></param>
        /// <param name="lsb"></param>
        public XBeePacketLength(int msb, int lsb) 
            : base(msb, lsb)
        {
        }

        public XBeePacketLength(int length)
            : base(length)
        {
        }

        public int GetLength()
        {
            return Get16BitValue();
        }
    }
}