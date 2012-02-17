using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public class NodeIdentifier
    {
        public const int MaxNodeIdentifierLength = 20;

        public static string Read(XBee xbee)
        {
            return Parse(xbee.Send(new AtCommand(AtCmd.NI)));
        }

        public static string Read(XBee sender, XBeeAddress16 remoteXbee)
        {
            return Parse(sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.NI)));
        }

        private static string Parse(AtCommandResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Failed to read node identifier");

            return Arrays.ToString(response.Value);
        }

        public static void Write(XBee xbee, string nodeIdentifier)
        {
            var value = Arrays.ToIntArray(nodeIdentifier, 0, MaxNodeIdentifierLength);
            var response = xbee.Send(new AtCommand(AtCmd.NI, value));

            if (!response.IsOk)
                throw new XBeeException("Failed to write node identifier");
        }

        public static void Write(XBee sender, XBeeAddress16 remoteXbee, string nodeIdentifier)
        {
            var value = Arrays.ToIntArray(nodeIdentifier, 0, MaxNodeIdentifierLength);
            var response = sender.Send(new RemoteAtCommand(remoteXbee, AtCmd.NI, value));

            if (!response.IsOk)
                throw new XBeeException("Failed to write node identifier");
        }
    }
}