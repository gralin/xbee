namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public enum AtResponseStatus
    {
        OK = 0,
        ERROR = 1,
        INVALID_COMMAND = 2,
        IVALID_PARAMETER = 3,

        /// <summary>
        /// Series 1 remote AT only according to spec.
        /// Also series 2 in 2x64 zb pro firmware
        /// </summary>
        NO_RESPONSE = 4
    }
}