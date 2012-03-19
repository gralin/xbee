using System;
using NETMF.OpenSource.XBee.Api.At;
using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee.
    /// Parses a Node Discover (ND) AT Command Response
    /// </summary>
    public class NodeDiscover : NodeInfo
    {
        public int Rssi { get; set; }

        public static NodeDiscover Parse(XBeeResponse response)
        {
            return Parse(response as AtResponse);
        }

        public static NodeDiscover Parse(AtResponse response)
        {
            if (response.Command != (ushort) Common.AtCmd.NodeDiscover)
                throw new ArgumentException("This method is only applicable for the ND command");

            // empty response is received after the last disovered node
            // this happens only with Wpan nodes, not Zigbee
            if (response.Value == null || response.Value.Length == 0)
                return null;

            var input = new InputStream(response.Value);

            var frame = new NodeDiscover
            {
                NetworkAddress = new XBeeAddress16(input.Read(2)),
                SerialNumber = new XBeeAddress64(input.Read(8)),
                Rssi = -1*input.Read()
            };

            byte ch;

            // NI is terminated with 0
            while ((ch = input.Read()) != 0)
                if (ch > 32 && ch < 126)
                    frame.NodeIdentifier += (char)ch;

            return frame;
        }

        public override string ToString()
        {
            return "networkAddress=" + NetworkAddress
                   + ",serialNumber=" + SerialNumber
                   + ",nodeIdentifier=" + NodeIdentifier
                   + ",rssi=" + Rssi + "dBi";
        }
    }
}