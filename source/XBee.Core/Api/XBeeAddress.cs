using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public abstract class XBeeAddress
    {
        public abstract int[] GetAddress();

        public override string ToString()
        {
            return ByteUtils.ToBase16(GetAddress());
        }
    }
}