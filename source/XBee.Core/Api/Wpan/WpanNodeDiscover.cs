using System;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Wpan
{
    /// <summary>
    /// Series 1 XBee.
    /// Parses a Node Discover (ND) AT Command Response
    /// </summary>
    public class WpanNodeDiscover
    {
        public XBeeAddress16 NodeAddress { get; set; }
        public XBeeAddress64 NodeSerial { get; set; }
        public string NodeIdentifier { get; set; }
        public int Rssi { get; set; }

        public static WpanNodeDiscover Parse(XBeeResponse response)
        {
            return Parse(response as AtResponse);
        }

        public static WpanNodeDiscover Parse(AtResponse response)
        {
            if (response.Command != AtCmd.NodeDiscover)
                throw new ArgumentException("This method is only applicable for the ND command");

            // empty response is received after the last disovered node
            // this happens only with Wpan nodes, not Zigbee
            if (response.Value == null || response.Value.Length == 0)
                return null;

            var input = new InputStream(response.Value);

            var frame = new WpanNodeDiscover
            {
                NodeAddress = new XBeeAddress16(input.Read(2)),
                NodeSerial = new XBeeAddress64(input.Read(8)),
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
            return "networkAddress=" + NodeAddress
                   + ",serialNumber=" + NodeSerial
                   + ",nodeIdentifier=" + NodeIdentifier
                   + ",rssi=" + Rssi + "dBi";
        }
    }
}