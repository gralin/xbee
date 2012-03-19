using NETMF.OpenSource.XBee.Api.At;

namespace NETMF.OpenSource.XBee.Api.Common
{
    public static class SaveSettings
    {
        public static void Write(XBee xbee)
        {
            var request = xbee.Send2(AtCmd.Write);
            Parse(request.GetResponse());
        }

        public static void Write(XBee sender, XBeeAddress remoteXbee)
        {
            var request = sender.Send2(AtCmd.Write).To(remoteXbee);
            Parse((AtResponse) request.GetResponse());
        }

        public static void Parse(AtResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Failed to save settings");
        }
    }
}