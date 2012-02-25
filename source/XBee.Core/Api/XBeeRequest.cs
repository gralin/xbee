using System;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public abstract class XBeeRequest
    {
        public abstract ApiId ApiId { get; }
        public byte FrameId { get; set; }

        protected XBeeRequest()
        {
            FrameId = PacketIdGenerator.DefaultId;
        }

        // TODO create XBeePacket(XBeeRequest) constructor and move operation there
        public XBeePacket GetXBeePacket()
        {
            var frameData = GetFrameData();

            if (frameData == null)
                throw new Exception("frame data is null");
        
            // TODO xbee packet should handle api/frame id
            var packet = new XBeePacket(frameData);

            return packet;
        }

        public abstract byte[] GetFrameData();

        public override string ToString()
        {
            return "ApiId=" + ByteUtils.ToBase16((byte)ApiId) + 
                   ",FrameId=" + ByteUtils.ToBase16(FrameId);
        }

        // TODO clear method to reuse request
    }
}