namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class CountLimitTerminator : PacketTerminator
    {
        private int _maxPacketCount;

        public int MaxPacketCount
        {
            get { return _maxPacketCount; }
            set
            {
                _maxPacketCount = value;
                RemainingPacketCount = value;
            }
        }

        public int RemainingPacketCount { get; private set; }

        public CountLimitTerminator(int maxPacketCount)
        {
            MaxPacketCount = maxPacketCount;
        }

        public override bool Terminate(XBeeResponse response)
        {
            RemainingPacketCount--;

            if (RemainingPacketCount > 0)
                return false;

            Finished.Set();
            return true;
        }
    }
}