using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents a XBee Address.
    /// </summary>
    public class HardwareVersion
    {
        public enum RadioType
        {
            UNKNOWN,
            SERIES1 = 0x17,
            SERIES1_PRO = 0x18,
            SERIES2 = 0x19,
            SERIES2_PRO = 0x1a,
            SERIES2B_PRO = 0x1e
        }

        public static RadioType Parse(AtCommandResponse response)
        {
            if (response.GetCommand() != "HV")
                throw new ArgumentException("This is only applicable to the HV command");

            if (!response.IsOk)
                throw new XBeeException("Attempt to query HV parameter failed");

            var radioType = (RadioType) response.Value[0];

            switch (radioType)
            {
                case RadioType.SERIES1:
                case RadioType.SERIES1_PRO:
                case RadioType.SERIES2:
                case RadioType.SERIES2_PRO:
                case RadioType.SERIES2B_PRO:
                    return radioType;

                default:
                    return RadioType.UNKNOWN;
            }
        }
    }
}