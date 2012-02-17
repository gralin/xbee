using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class Firmware
    {
        public static string Read(XBee xbee)
        {
            var response = xbee.Send(new AtCommand(AtCmd.VR));

            if (!response.IsOk)
                throw new XBeeException("Attempt to query HV parameter failed");

            return ByteUtils.ToBase16(response.Value);
        }
    }
}