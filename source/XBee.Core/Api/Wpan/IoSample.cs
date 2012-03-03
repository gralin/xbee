using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Provides access to XBee's 8 Digital (0-7) and 6 Analog (0-5) IO pins.
    /// </summary>
    public class IoSample
    {
        private readonly ushort[] _analog;
        private readonly ushort _digital;

        public IoSample(ushort[] analog, ushort digital)
        {
            _analog = analog;
            _digital = digital;
        }

        public ushort GetValue(Pin.Analog pin)
        {
            return _analog[(byte)pin];
        }

        public bool GetValue(Pin.Digital pin)
        {
            return UshortUtils.GetBit(_digital, (byte)pin);
        }
    }
}