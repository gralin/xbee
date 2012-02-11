namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee.  Super class of all Receive packets.
    /// </summary>
    /// <remarks>
    ///  Note: ZNet RX packets do not include RSSI since it is a mesh network and potentially requires several
    /// hops to get to the destination.  The RSSI of the last hop is available using the DB AT command.
    /// If your network is not mesh (i.e. composed of a single coordinator and end devices -- no routers) 
    /// then the DB command should provide accurate RSSI.
    /// </remarks>
    public abstract class ZNetRxBaseResponse : XBeeResponse
    {
        public enum Options
        {
            PACKET_ACKNOWLEDGED = 0x01,
            BROADCAST_PACKET = 0x02
        }

        // TODO where is frame id??

        public XBeeAddress64 RemoteAddress64 { get; set; }
        public XBeeAddress16 RemoteAddress16 { get; set; }
        public Options Option { get; set; }

        protected void ParseAddress (IPacketParser parser)
        {
            RemoteAddress64 = parser.ParseAddress64();
            RemoteAddress16 = parser.ParseAddress16();
        }

        protected void ParseOption (IPacketParser parser)
        {
            Option = (Options) parser.Read("ZNet RX Response Option");
        }

        public override string ToString()
        {
            return base.ToString() +
                   ",remoteAddress64=" + RemoteAddress64 +
                   ",remoteAddress16=" + RemoteAddress16 +
                   ",option=" + Option;
        }
    }
}