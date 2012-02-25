namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// RF module status messages are sent from the module in response to specific conditions.
    /// API ID: 0x8a
    /// </summary>
    public class ModemStatusResponse : XBeeResponse, INoRequestResponse
    {
        #region Status enum

        public enum Status
        {
            HardwareReset = 0,
            WatchdogTmerReset = 1,
            Associated = 2,
            Disassociated = 3,
            SynchronizationLost = 4,
            CoordinatorRealigment = 5,
            CoordinatorStarted = 6
        }

        #endregion

        public Status ResponseStatus { get; set; }

        public override void Parse(IPacketParser parser)
        {
            ResponseStatus = (Status) parser.Read("Modem Status");
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",status=" + ResponseStatus;
        }
    }
}