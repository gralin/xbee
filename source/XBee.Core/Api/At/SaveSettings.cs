namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class SaveSettings
    {
        public static void Write(XBee xbee)
        {
            Parse(xbee.Send(new AtCommand(AtCmd.WR)));
        }

        public static void Write(XBee sender, XBeeAddress16 remoteXbee)
        {
            Parse(sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.WR)));
        }

        public static void Parse(AtCommandResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Failed to save settings");
        }
    }
}