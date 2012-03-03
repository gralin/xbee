using Gadgeteer.Modules.GHIElectronics.Api.At;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class AtResponseFilter : PacketIdFilter
    {
        private readonly AtCmd _atCmd;
        private bool _finished;

        public AtResponseFilter(AtCmd atCommand, int packetId)
            : base(packetId)
        {
            _atCmd = atCommand;
        }

        public AtResponseFilter(AtCommand atRequest)
            : this(atRequest.Command, atRequest.FrameId)
        {
        }

        public override bool Accepted(XBeeResponse packet)
        {
            if (!base.Accepted(packet))
                return false;

            if (!(packet is AtResponse))
                return false;

            var accepted = (packet as AtResponse).Command == _atCmd;

            _finished = accepted;
            return accepted;
        }

        public override bool Finished()
        {
            return _finished;
        }
    }
}