using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Represents all XBee responses that contain a frame id
    /// </summary>
    public abstract class XBeeFrameIdResponse : XBeeResponse
    {
        public byte FrameId { get; set; }

        public override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("Frame Id");
        }

        public override string ToString()
        {
            return base.ToString() + ",frameId=" + ByteUtils.ToBase16(FrameId);
        }
    }
}