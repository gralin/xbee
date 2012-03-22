namespace NETMF.OpenSource.XBee.Api.Common
{
    public enum ResetMode
    {
        /// <summary>
        /// Reset module.
        /// </summary>
        Software,

        /// <summary>
        /// Reset network layer parameters.
        /// </summary>
        Network,
        
        /// <summary>
        /// Restore module parameters to factory defaults.
        /// </summary>
        RestoreDefaults
    }
}