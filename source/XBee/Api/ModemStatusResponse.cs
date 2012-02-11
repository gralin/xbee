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
            HARDWARE_RESET = 0,
            WATCHDOG_TMER_RESET = 1,
            ASSOCIATED = 2,
            DISASSOCIATED = 3,
            SYNCHRONIZATION_LOST = 4,
            COORDINATOR_REALIGMENT = 5,
            COORDINATOR_STARTED = 6
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