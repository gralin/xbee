using NETMF.OpenSource.XBee.Util;

namespace NETMF.OpenSource.XBee.Api.Zigbee
{
    /// <summary>
    /// Series 2 XBee.  Sends a packet to a remote radio.  
    /// The remote radio receives the packet as a ExplicitRxResponse packet.
    /// API ID: 0x11
    /// </summary>
    public class ExplicitTxRequest : TxRequest
    {
        public enum Endpoint
        {
            ZigbeeDeviceObject = 0,
            Command = 0xe6,
            Data = 0xe8
        }

        public enum ClusterIds
        {
            TransparentSerial = 0x11,
            SerialLoopback = 0x12,
            IOSample = 0x92,
            XbeeSensor = 0x94,
            NodeIdentification = 0x95
        }

        public byte SourceEndpoint { get; set; }
        public byte DestinationEndpoint { get; set; }
        public ushort ClusterId { get; set; }
        public ushort ProfileId { get; set; }

        public static ushort ZnetProfileId = 0xC105;
        public static ushort ZdoProfileId = 0x0000;

        // this is one big ctor ;)

        public ExplicitTxRequest(XBeeAddress64 destSerial, XBeeAddress16 destAddress, byte[] payload, byte srcEndpoint, byte destEndpoint, ushort clusterId, ushort profileId) 
            : base(destSerial, payload)
        {
            DestinationAddress = destAddress;
            SourceEndpoint = srcEndpoint;
            DestinationEndpoint = destEndpoint;
            ClusterId = clusterId;
            ProfileId = profileId;
        }

        protected override void GetFrameHeader(OutputStream output)
        {
            base.GetFrameHeader(output);

            // source endpoint
            output.Write(SourceEndpoint);
            // dest endpoint
            output.Write(DestinationEndpoint);
            // cluster id
            output.Write(ClusterId);
            // profile id
            output.Write(ProfileId);
        }

        public override ApiId ApiId
        {
            get { return ApiId.ZnetExplicitTxRequest; }
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