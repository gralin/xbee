namespace NETMF.OpenSource.XBee.Api.Wpan
{
    /// <summary>
    /// Value returned by <see cref="WpanAtCmd.SleepMode"/> command.
    /// </summary>
    public enum SleepMode
    {
        NoSleep = 0,
        PinHibernate = 1,
        PinDoze = 2,
        CyclicSleepRemote = 4,
        CyclicSleepRemoteWithPinWakeUp = 5,
        
        /// <summary>
        /// For backwards compatibility with firmware v1.x6 only. 
        /// Use <see cref="WpanAtCmd.CoordinatorEnable"/> command.
        /// </summary>
        SleepCoordinator = 6
    }
}