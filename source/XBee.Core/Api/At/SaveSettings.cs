namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class SaveSettings
    {
        public static void Write(XBee xbee)
        {
            Parse(xbee.Send(AtCmd.Write));
        }

        public static void Write(XBee sender, XBeeAddress remoteXbee)
        {
            Parse(sender.Send(AtCmd.Write, remoteXbee));
        }

        public static void Parse(AtResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Failed to save settings");
        }
    }
}