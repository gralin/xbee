namespace NETMF.OpenSource.XBee.Api
{
    public enum ModemStatus : byte
    {
        HardwareReset = 0,
        WatchdogTimerReset = 1,

        /// <summary>
        /// Joined network (routers and end devices)
        /// </summary>
        Associated = 2,

        Disassociated = 3,
        SynchronizationLost = 4,
        CoordinatorRealigment = 5,
        CoordinatorStarted = 6,

        /// <summary>
        /// Network security key was updated
        /// </summary>
        SecurityKeyUpdated = 7,

        /// <remarks>
        /// Voltage supply limit exceeded (PRO S2B only)
        /// </remarks>
        VoltageLimitExceeded = 0x0D,

        /// <summary>
        /// Modem configuration changed while join in progress
        /// </summary>
        ConfigChangedWhileJoining = 0x11,

        /// <summary>
        /// Stack error for values 0x80+
        /// </summary>
        StackError = 0x80
    }
}