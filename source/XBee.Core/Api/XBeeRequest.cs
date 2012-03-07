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

        public abstract byte[] GetFrameData();

        public override string ToString()
        {
            return "ApiId=" + ByteUtils.ToBase16((byte)ApiId) + 
                   ",FrameId=" + ByteUtils.ToBase16(FrameId);
        }
    }
}