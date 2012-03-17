using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api.At
{
    public class NodeIdentifier
    {
        public const byte MaxNodeIdentifierLength = 20;

        public static string Read(XBee xbee)
        {
            return Parse(xbee.Send(AtCmd.NodeIdentifier));
        }

        public static string Read(XBee sender, XBeeAddress remoteXbee)
        {
            return Parse(sender.Send(AtCmd.NodeIdentifier, remoteXbee));
        }

        private static string Parse(AtResponse response)
        {
            if (!response.IsOk)
                throw new XBeeException("Failed to read node identifier");

            return Arrays.ToString(response.Value);
        }

        public static void Write(XBee xbee, string nodeIdentifier)
        {
            var value = Arrays.ToByteArray(nodeIdentifier, 0, MaxNodeIdentifierLength);
            var response = xbee.Send(AtCmd.NodeIdentifier, value);

            if (!response.IsOk)
                throw new XBeeException("Failed to write node identifier");
        }

        public static void Write(XBee sender, XBeeAddress remoteXbee, string nodeIdentifier)
        {
            var value = Arrays.ToByteArray(nodeIdentifier, 0, MaxNodeIdentifierLength);
            var response = sender.Send(AtCmd.NodeIdentifier, remoteXbee, value);

            if (!response.IsOk)
                throw new XBeeException("Failed to write node identifier");
        }
    }
}