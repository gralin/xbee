using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// This frame is received when a module transmits a node identification message to identify itself (when AO=0).
    /// The data portion of this frame is similar to a network discovery response frame (see <see cref="ZBNodeDiscover"/>).
    /// </summary>
    /// <example>
    /// If the commissioning push button is pressed on a remote router device with 64-bit address 0x0013A20040522BAA, 
    /// 16-bit address 0x7D84, and default NI string, the preceding node identification indicator would be received. 
    /// Please note that 00 03 00 00 appears before the checksum with the DD value only if ATNO & 0x01.
    /// </example>
    public class ZNetNodeIdentificationResponse : XBeeResponse
    {
        public enum PacketOption
        {
            /// <summary>
            /// Packet Acknowledged
            /// </summary>
            Ack = 0x01,

            /// <summary>
            /// Packet was a broadcast packet
            /// </summary>
            Broadcast = 0x02
        }

        public enum SourceActions
        {
            /// <summary>
            /// Frame sent by node identific ation pushbutton event (see D0 command)
            /// </summary>
            Pushbutton = 1,

            /// <summary>
            /// Frame sent after joining event occurred (see <see cref="AtCmd.JoinNotification"/>).
            /// </summary>
            Joining = 2,

            /// <summary>
            /// Frame sent after power cycle event occurred (see <see cref="AtCmd.JoinNotification"/>).
            /// </summary>
            PowerCycle = 3
        }

        // serial and network address of node that transmited this packet
        // this will be equal to remote serial and address if there were
        // not hops in between
        public XBeeAddress64 SenderSerial { get; set; }
        public XBeeAddress16 SenderAddress { get; set; }

        // serial and netowork address of remote node that was identified
        public XBeeAddress64 RemoteSerial { get; set; }
        public XBeeAddress16 RemoteAddress { get; set; }

        public PacketOption Option { get; set; }
        
        // these properties are all regarding the remote node
        public string NodeIdentifier { get; set; }
        public XBeeAddress16 ParentAddress { get; set; }
        public DeviceType DeviceType { get; set; }
        public SourceActions SourceAction { get; set; }

        /// <summary>
        /// Set to Digi's application profile ID.
        /// </summary>
        public ushort ProfileId { get; set; }

        /// <summary>
        /// Set to Digi's Manufacturer ID.
        /// </summary>
        public ushort MfgId { get; set; }

        public override void Parse(IPacketParser parser)
        {
            SenderSerial = parser.ParseAddress64();
            SenderAddress = parser.ParseAddress16();

            Option = (PacketOption) parser.Read("Option");

            // again with the addresses
            RemoteSerial = parser.ParseAddress64();
            RemoteAddress = parser.ParseAddress16();

            var nodeIdentifier = string.Empty;
            byte ch;

            // NI is terminated with 0
            while ((ch = parser.Read("Node Identifier")) != 0)
            {
                if (ch > 32 && ch < 126)
                    nodeIdentifier += (char) ch;
            }

            NodeIdentifier = nodeIdentifier;
            ParentAddress = parser.ParseAddress16();
            DeviceType = (DeviceType) parser.Read("Device Type");
            SourceAction = (SourceActions) parser.Read("Source Action");
            ProfileId = UshortUtils.ToUshort(parser.Read("Profile MSB"), parser.Read("Profile LSB"));
            MfgId = UshortUtils.ToUshort(parser.Read("MFG MSB"), parser.Read("MFG LSB"));
        }

        public override string ToString()
        {
            return base.ToString()
                   + ", senderAddress=" + SenderAddress
                   + ", senderSerial=" + SenderSerial
                   + ", remoteAddress=" + RemoteAddress
                   + ", remoteSerial=" + RemoteSerial
                   + ", deviceType=" + DeviceType
                   + ", mfgId=" + MfgId
                   + ", nodeIdentifier=" + NodeIdentifier
                   + ", option=" + Option
                   + ", parentAddress=" + ParentAddress
                   + ", profileId=" + ProfileId
                   + ", sourceAction=" + SourceAction;
        }
    }
}