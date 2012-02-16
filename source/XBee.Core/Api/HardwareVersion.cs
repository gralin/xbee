using System;
using System.Collections;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents a XBee Address.
    /// </summary>
    public class HardwareVersion
    {
        public enum RadioType
        {
            UNKNOWN = 0x00,
            SERIES1 = 0x17,
            SERIES1_PRO = 0x18,
            SERIES2 = 0x19,
            SERIES2_PRO = 0x1a,
            SERIES2B_PRO = 0x1e
        }

        private static readonly Hashtable RadioTypeNames;

        static HardwareVersion()
        {
            RadioTypeNames = new Hashtable
            {
                {RadioType.UNKNOWN,"Unknown"},
                {RadioType.SERIES1,"Series 1"},
                {RadioType.SERIES1_PRO,"Series 1 Pro"},
                {RadioType.SERIES2,"Series 2"},
                {RadioType.SERIES2_PRO,"Series 2 Pro"},
                {RadioType.SERIES2B_PRO,"Series 2B Pro"},
            };
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

        public static string GetName(RadioType radiotype)
        {
            return (string) RadioTypeNames[radiotype];
        }
    }
}