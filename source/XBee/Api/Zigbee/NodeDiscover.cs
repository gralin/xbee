﻿using System;
using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee. Parses a Node Discover (ND) AT Command Response
    /// </summary>
    public class NodeDiscover : Common.NodeDiscover
    {
        public XBeeAddress16 Parent { get; set; }
        public NodeType NodeType { get; set; }
        public byte Status { get; set; }
        public byte[] ProfileId { get; set; }
        public byte[] MfgId { get; set; }

        public string NodeTypeName
        {
            get
            {
                switch (NodeType)
                {
                    case NodeType.Coordinator:
                        return "Coordinator";
                    case NodeType.Router:
                        return "Router";
                    case NodeType.EndDevice:
                        return "End device";
                    default:
                        return "Unknown";
                }
            }
        }

        public static NodeDiscover Parse(XBeeResponse response)
        {
            return Parse(response as AtResponse);
        }

        public static NodeDiscover Parse(AtResponse response)
        {
            if (response.Command != (ushort) Common.AtCmd.NodeDiscover)
                throw new ArgumentException("This method is only applicable for the ND command");

            var input = new InputStream(response.Value);

            var frame = new NodeDiscover
            {
                NodeInfo = new NodeInfo
                {
                    NetworkAddress = new XBeeAddress16(input.Read(2)),
                    SerialNumber = new XBeeAddress64(input.Read(8))   
                }
            };

            byte ch;

            // NI is terminated with 0
            while ((ch = input.Read()) != 0)
                if (ch > 32 && ch < 126)
                    frame.NodeInfo.NodeIdentifier += (char)ch;

            frame.Parent = new XBeeAddress16(input.Read(2));
            frame.NodeType = (NodeType) input.Read();
            frame.Status = input.Read();
            frame.ProfileId = input.Read(2);
            frame.MfgId = input.Read(2);

            return frame;
        }

        public override string ToString()
        {
            return NodeTypeName
                + ", " + base.ToString()
                + ", parent=" + Parent;
        }
    }
}