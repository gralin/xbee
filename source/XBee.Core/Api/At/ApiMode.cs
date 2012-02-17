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
            return Parse(xbee.Send(new AtCommand(AtCmd.AP)));
        }

        public static ApiModes Read(XBee sender, XBeeAddress16 remoteXbee)
        {
            return Parse(sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.AP)));
        }

        public static ApiModes Parse(AtCommandResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Attempt to query AP parameter failed");

            return (ApiModes) response.Value[0];
        }

        public static void Write(XBee xbee, ApiModes mode)
        {
            var response = xbee.Send(new AtCommand(AtCmd.AP, (int)mode));

            if (!response.IsOk)
                throw new XBeeException("Failed to write api mode");
        }

        public static void Write(XBee sender, XBeeAddress16 remoteXbee, ApiModes mode)
        {
            var response = sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.AP, new[] {(int)mode}));

            if (!response.IsOk)
                throw new XBeeException("Failed to write api mode");
        }

        public static string GetName(ApiModes apiMode)
        {
            return (string)ApiModeNames[apiMode];
        }
    }
}