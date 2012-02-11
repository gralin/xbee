using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public abstract class XBeeRequest
    {
        public static int DEFAULT_FRAME_ID = 1;
	    // XBee will not generate a TX Status Packet if this frame id sent
	    public static int NO_RESPONSE_FRAME_ID = 0;

        public virtual ApiId ApiId { get; set; }
        public int FrameId { get; set; }

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

        public abstract int[] GetFrameData();

        public override string ToString()
        {
            return "ApiId=" + ApiId + 
                   ",FrameId=" + FrameId;
        }

        // TODO clear method to reuse request
    }
}