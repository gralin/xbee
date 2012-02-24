using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee.  Sends a packet to a remote radio.  
    /// The remote radio receives the packet as a ZNetExplicitRxResponse packet.
    /// API ID: 0x11
    /// </summary>
    public class ZNetExplicitTxRequest : ZNetTxRequest
    {
        public enum Endpoint
        {
            ZDO_ENDPOINT = 0,
            COMMAND = 0xe6,
            DATA = 0xe8
        }

        public enum ClusterIds
        {
            TRANSPARENT_SERIAL = 0x11,
            SERIAL_LOOPBACK = 0x12,
            IO_SAMPLE = 0x92,
            XBEE_SENSOR = 0x94,
            NODE_IDENTIFICATION = 0x95
        }

        public int SourceEndpoint { get; set; }
        public int DestinationEndpoint { get; set; }
        public ushort ClusterId { get; set; }
        public ushort ProfileId { get; set; }

        public static ushort ZnetProfileId = UshortUtils.ToUshort(0xC1, 0x05);
        public static ushort ZdoProfileId = UshortUtils.ToUshort(0x00, 0x00);

        // this is one big ctor ;)

        public ZNetExplicitTxRequest(XBeeAddress64 destSerial, XBeeAddress16 destAddress, int[] payload, int srcEndpoint, int destEndpoint, ushort clusterId, ushort profileId,
            Options option = Options.UNICAST, int broadcastRadius = DEFAULT_BROADCAST_RADIUS, int frameId = PacketIdGenerator.DefaultId) 
            : base(destSerial, payload, broadcastRadius, option, frameId)
        {
            DestinationAddress = destAddress;
            SourceEndpoint = srcEndpoint;
            DestinationEndpoint = destEndpoint;
            ClusterId = clusterId;
            ProfileId = profileId;
        }

        public override int[] GetFrameData()
        {
            // get frame id from tx request
            var frameData = GetFrameDataAsIntArrayOutputStream();

            // api id
            frameData.Write((byte)ApiId);

            // frame id (arbitrary byte that will be sent back with ack)
            frameData.Write(FrameId);

            // add 64-bit dest address
            frameData.Write(DestinationSerial.Address);

            // add 16-bit dest address
            frameData.Write(DestinationAddress.Address);

            // source endpoint
            frameData.Write(SourceEndpoint);
            // dest endpoint
            frameData.Write(DestinationEndpoint);
            // cluster id
            frameData.Write(ClusterId);
            // profile id
            frameData.Write(ProfileId);

            // write broadcast radius
            frameData.Write(BroadcastRadius);

            // write options byte
            frameData.Write((byte)Option);

            frameData.Write(Payload);

            return frameData.ToArray();
        }

        public override ApiId ApiId
        {
            get { return ApiId.ZNET_EXPLICIT_TX_REQUEST; }
        }

        public override string ToString()
        {
            return base.ToString() +
                ",sourceEndpoint=" + ByteUtils.ToBase16(SourceEndpoint) +
                ",destinationEndpoint=" + ByteUtils.ToBase16(DestinationEndpoint) +
                ",clusterId=" + ByteUtils.ToBase16(ClusterId) +
                ",profileId=" + ByteUtils.ToBase16(ProfileId);
        }
    }
}