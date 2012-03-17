namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class PacketIdFilter : PacketTypeFilter
    {
        public int ExpectedPacketId;
        private bool _finished;

        public PacketIdFilter(int packetId)
            : base(typeof(XBeeFrameIdResponse))
        {
            ExpectedPacketId = packetId;
        }

        public PacketIdFilter(XBeeRequest request)
            : this (request.FrameId)
        {
        }

        public override bool Accepted(XBeeResponse packet)
        {
            if (!base.Accepted(packet))
                return false;

            var frameIdResponse = (XBeeFrameIdResponse)packet;

            var accepted = ExpectedPacketId == PacketIdGenerator.DefaultId 
                || frameIdResponse.FrameId == ExpectedPacketId;

            _finished = accepted;
            return accepted;
        }

        public override bool Finished()
        {
            return _finished;
        }
    }
}