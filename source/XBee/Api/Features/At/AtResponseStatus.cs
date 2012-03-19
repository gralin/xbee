namespace NETMF.OpenSource.XBee.Api
{
    public enum AtResponseStatus
    {
        Ok = 0,
        Error = 1,
        InvalidCommand = 2,
        IvalidParameter = 3,

        /// <summary>
        /// Series 1 remote AT only according to spec.
        /// Also series 2 in 2x64 zb pro firmware
        /// </summary>
        TransmissionFailed = 4
    }
}