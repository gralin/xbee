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
            var response = xbee.Send(new AtCommand(AtCmd.AP));

            if (!response.IsOk)
                throw new XBeeException("Attempt to query AP parameter failed");

            return (ApiModes)response.Value[0];
        }

        public static void Write(XBee xbee, ApiModes mode)
        {
            var response = xbee.Send(new AtCommand(AtCmd.AP, (int)mode));

            if (!response.IsOk)
                throw new XBeeException("Failed to write api mode");
        }

        public static string GetName(HardwareVersions radiotype)
        {
            return (string) ApiModeNames[radiotype];
        }
    }
}