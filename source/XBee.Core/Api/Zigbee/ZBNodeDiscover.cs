using System;
using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee. Parses a Node Discover (ND) AT Command Response
    /// </summary>
    public class ZBNodeDiscover
    {
        public enum DeviceTypes
        {
            DEV_TYPE_COORDINATOR = 0,
            DEV_TYPE_ROUTER = 1,
            DEV_TYPE_END_DEVICE = 2
        }

        public XBeeAddress64 NodeAddress64 { get; set; }
        public XBeeAddress16 NodeAddress16 { get; set; }
        public string NodeIdentifier { get; set; }
        public XBeeAddress16 Parent { get; set; }
        public DeviceTypes DeviceType { get; set; }
        public int Status { get; set; }
        public int[] ProfileId { get; set; }
        public int[] MfgId { get; set; }

        public string DeviceTypeName
        {
            get
            {
                switch (DeviceType)
                {
                    case DeviceTypes.DEV_TYPE_COORDINATOR:
                        return "Coordinator";
                    case DeviceTypes.DEV_TYPE_ROUTER:
                        return "Router";
                    case DeviceTypes.DEV_TYPE_END_DEVICE:
                        return "End device";
                    default:
                        return "Unknown";
                }
            }
        }

        public static ZBNodeDiscover Parse(XBeeResponse response)
        {
            return Parse(response as AtResponse);
        }

        public static ZBNodeDiscover Parse(AtResponse response)
        {
            if (response.Command != AtCmd.NodeDiscover)
                throw new ArgumentException("This method is only applicable for the ND command");

            var input = new InputStream(response.Value);

            var frame = new ZBNodeDiscover
            {
                NodeAddress16 = new XBeeAddress16(input.Read(2)),
                NodeAddress64 = new XBeeAddress64(input.Read(8))
            };

            var nodeIdentifier = string.Empty;
            
            int ch;

            // NI is terminated with 0
            while ((ch = input.Read()) != 0)
            {
                if (ch > 32 && ch < 126)
                    nodeIdentifier += (char)ch;
            }

            frame.NodeIdentifier = nodeIdentifier;
            frame.Parent = new XBeeAddress16(input.Read(2));
            frame.DeviceType = (DeviceTypes) input.Read();
            // TODO: this is being reported as 1 (router) for my end device
            frame.Status = input.Read();
            frame.ProfileId = input.Read(2);
            frame.MfgId = input.Read(2);

            return frame;
        }

        public override string ToString()
        {
            return DeviceTypeName 
                + ", SerialNumber = " + NodeAddress64 
                + ", NetworkAddress = " + NodeAddress16;
        }
    }
}