using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class Firmware
    {
        public static string Read(XBee xbee)
        {
            return Parse(xbee.Send(new AtCommand(AtCmd.VR)));
        }

        public static string Read(XBee sender, XBeeAddress16 remoteXbee)
        {
            return Parse(sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.VR)));
        }

        public static string Parse(AtCommandResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Attempt to query HV parameter failed");

            return ByteUtils.ToBase16(response.Value);
        }
    }
}