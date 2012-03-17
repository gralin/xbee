using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api.At
{
    public static class Firmware
    {
        public static string Read(XBee xbee)
        {
            return Parse(xbee.Send(AtCmd.FirmwareVersion));
        }

        public static string Read(XBee sender, XBeeAddress remoteXbee)
        {
            return Parse(sender.Send(AtCmd.FirmwareVersion, remoteXbee));
        }

        public static string Parse(AtResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Attempt to query HV parameter failed");

            return ByteUtils.ToBase16(response.Value);
        }
    }
}