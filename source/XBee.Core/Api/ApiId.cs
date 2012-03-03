namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public enum ApiId
    {
        /// <summary>
        /// API ID: 0x0
        /// </summary>
        TxRequest64 = 0x0,

        /// <summary>
        /// API ID: 0x1
        /// </summary>
        TxRequest16 = 0x1,

        /// <summary>
        /// API ID: 0x08
        /// </summary>
        AtCommand = 0x08,

        /// <summary>
        /// API ID: 0x09
        /// </summary>
        AtCommandQueue = 0x09,

        /// <summary>
        /// API ID: 0x17
        /// </summary>
        RemoteAtRequest = 0x17,

        /// <summary>
        /// API ID: 0x10
        /// </summary>
        ZnetTxRequest = 0x10,

        /// <summary>
        /// API ID: 0x11
        /// </summary>
        ZnetExplicitTxRequest = 0x11,

        /// <summary>
        /// API ID: 0x80
        /// </summary>
        Rx64Response = 0x80,

        /// <summary>
        /// API ID: 0x81
        /// </summary>
        Rx16Response = 0x81,

        /// <summary>
        /// API ID: 0x82
        /// </summary>
        Rx64IOResponse = 0x82,

        /// <summary>
        /// API ID: 0x83
        /// </summary>
        Rx16IOResponse = 0x83,

        /// <summary>
        /// API ID: 0x88
        /// </summary>
        AtResponse = 0x88,

        /// <summary>
        /// API ID: 0x89
        /// </summary>
        TxStatusResponse = 0x89,

        /// <summary>
        /// API ID: 0x8a
        /// </summary>
        ModemStatusResponse = 0x8a,

        /// <summary>
        /// API ID: 0x90
        /// </summary>
        ZnetRxResponse = 0x90,

        /// <summary>
        /// API ID: 0x91
        /// </summary>
        ZnetExplicitRxResponse = 0x91,

        /// <summary>
        /// API ID: 0x8b
        /// </summary>
        ZnetTxStatusResponse = 0x8b,

        /// <summary>
        /// API ID: 0x97
        /// </summary>
        RemoteAtResponse = 0x97,

        /// <summary>
        /// API ID: 0x92
        /// </summary>
        ZnetIOSampleResponse = 0x92,

        /// <summary>
        /// API ID: 0x95
        /// </summary>
        ZnetIONodeIdentifierResponse = 0x95,

        /// <summary>
        /// Indicates that we've parsed a packet for which we didn't 
        /// know how to handle the API type. This will be parsed 
        /// into a GenericResponse   
        /// </summary>
        Unknown = 0xff,

        /// <summary>
        /// This is returned if an error occurs during packet parsing
        /// and does not correspond to a XBee API ID.
        /// </summary>
        ErrorResponse = -1
    }
}