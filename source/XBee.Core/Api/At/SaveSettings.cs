namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class SaveSettings
    {
         public static void Write(XBee xbee)
         {
             var response = xbee.Send(new AtCommand(AtCmd.WR));

             if (!response.IsOk)
                 throw new XBeeException("Failed to save settings");
         }
    }
}