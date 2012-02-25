namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class CountLimitTerminator : PacketTerminator
    {
        private byte _maxPacketCount;

        public byte MaxPacketCount
        {
            get { return _maxPacketCount; }
            set
            {
                _maxPacketCount = value;
                RemainingPacketCount = value;
            }
        }

        public int RemainingPacketCount { get; private set; }

        public CountLimitTerminator(byte maxPacketCount)
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