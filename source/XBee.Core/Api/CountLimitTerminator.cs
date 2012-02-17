namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// This class can is used to limit XBee.CollectResponses result to specified max count
    /// </summary>
    internal class CountLimitTerminator : ICollectTerminator
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

        public bool Stop(XBeeResponse response)
        {
            RemainingPacketCount--;
            return RemainingPacketCount <= 0;
        }
    }
}