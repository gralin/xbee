using System.Collections;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class HardwareVersion
    {
        private static readonly Hashtable HardwareVersionNames;

        static HardwareVersion()
        {
            HardwareVersionNames = new Hashtable
            {
                {HardwareVersions.UNKNOWN,"Unknown"},
                {HardwareVersions.SERIES1,"Series 1"},
                {HardwareVersions.SERIES1_PRO,"Series 1 Pro"},
                {HardwareVersions.SERIES2,"Series 2"},
                {HardwareVersions.SERIES2_PRO,"Series 2 Pro"},
                {HardwareVersions.SERIES2B_PRO,"Series 2B Pro"},
            };
        }

        public static HardwareVersions Read(XBee xbee)
        {
            return Parse(xbee.Send(AtCmd.HardwareVersion));
        }

        public static HardwareVersions Read(XBee sender, XBeeAddress16 remoteXBee)
        {
            return Parse(sender.Send(AtCmd.HardwareVersion, remoteXBee));
        }

        public static HardwareVersions Parse(AtResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Attempt to query remote HV parameter failed");

            return (HardwareVersions)response.Value[0];
        }

        public static string GetName(HardwareVersions radiotype)
        {
            return (string) HardwareVersionNames[radiotype];
        }
    }
}