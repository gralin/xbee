using System.Collections;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public static class ApiMode
    {
        private static readonly Hashtable ApiModeNames;

        static ApiMode()
        {
            ApiModeNames = new Hashtable
            {
                {ApiModes.Disabled,"Disabled"},
                {ApiModes.Enabled,"Enabled"},
                {ApiModes.EnabledWithEscaped,"EnabledWithEscaped"},
                {ApiModes.Unknown,"Unknown"}
            };
        }

        public static ApiModes Read(XBee xbee)
        {
            return Parse(xbee.Send(AtCmd.ApiEnable));
        }

        public static ApiModes Read(XBee sender, XBeeAddress16 remoteXbee)
        {
            return Parse(sender.Send(AtCmd.ApiEnable, remoteXbee));
        }

        public static ApiModes Parse(AtResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Attempt to query AP parameter failed");

            return (ApiModes) response.Value[0];
        }

        public static void Write(XBee xbee, ApiModes mode)
        {
            var response = xbee.Send(AtCmd.ApiEnable, new[]{(int)mode});

            if (!response.IsOk)
                throw new XBeeException("Failed to write api mode");
        }

        public static void Write(XBee sender, XBeeAddress16 remoteXbee, ApiModes mode)
        {
            var response = sender.Send(AtCmd.ApiEnable, remoteXbee, new[] {(int)mode});

            if (!response.IsOk)
                throw new XBeeException("Failed to write api mode");
        }

        public static string GetName(ApiModes apiMode)
        {
            return (string)ApiModeNames[apiMode];
        }
    }
}