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
        public DoubleByte ClusterId { get; set; }
        public DoubleByte ProfileId { get; set; }

        public static DoubleByte ZnetProfileId = new DoubleByte(0xC1, 0x05);
        public static DoubleByte ZdoProfileId = new DoubleByte(0, 0);

        // this is one big ctor ;)

        public ZNetExplicitTxRequest(int frameId, XBeeAddress64 dest64, XBeeAddress16 dest16, int broadcastRadius, 
            Options option, int[] payload, int srcEndpoint, int destEndpoint, DoubleByte clusterId, DoubleByte profileId) 
            : base(frameId, dest64, dest16, broadcastRadius, option, payload)
        {
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
            frameData.Write(DestAddr64.GetAddress());

            // add 16-bit dest address
            frameData.Write(DestAddr16.GetAddress());

            // add 16-bit dest address
            frameData.Write(DestAddr16.GetAddress());

            // source endpoint
            frameData.Write(SourceEndpoint);
            // dest endpoint
            frameData.Write(DestinationEndpoint);
            // cluster id msb
            frameData.Write(ClusterId.Msb);
            // cluster id lsb
            frameData.Write(ClusterId.Lsb);
            // profile id
            frameData.Write(ProfileId.Msb);
            frameData.Write(ProfileId.Lsb);

            // write broadcast radius
            frameData.Write(BroadcastRadius);

            // write options byte
            frameData.Write((byte)Option);

            frameData.Write(Payload);

            return frameData.GetIntArray();
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
                ",clusterId(msb)=" + ByteUtils.ToBase16(ClusterId.Msb) +
                ",clusterId(lsb)=" + ByteUtils.ToBase16(ClusterId.Lsb) +
                ",profileId(msb)=" + ByteUtils.ToBase16(ProfileId.Msb) +
                ",profileId(lsb)=" + ByteUtils.ToBase16(ProfileId.Lsb);
        }
    }
}