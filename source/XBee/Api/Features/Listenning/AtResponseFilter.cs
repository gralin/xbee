namespace NETMF.OpenSource.XBee.Api
{
    public class AtResponseFilter : PacketIdFilter
    {
        private readonly ushort _atCmd;
        private bool _finished;

        public AtResponseFilter(ushort atCommand, int packetId)
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

            var accepted = (ushort)(packet as AtResponse).Command == _atCmd;

            _finished = accepted;
            return accepted;
        }

        public override bool Finished()
        {
            return _finished;
        }
    }
}