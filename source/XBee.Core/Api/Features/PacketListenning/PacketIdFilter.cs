namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class PacketIdFilter : PacketTypeFilter
    {
        private readonly int _expectedPacketId;
        private bool _finished;

        public PacketIdFilter(int packetId)
            : base(typeof(XBeeFrameIdResponse))
        {
            _expectedPacketId = packetId;
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

            var accepted = _expectedPacketId == PacketIdGenerator.DefaultId 
                || frameIdResponse.FrameId == _expectedPacketId;

            _finished = accepted;
            return accepted;
        }

        public override bool Finished()
        {
            return _finished;
        }
    }
}