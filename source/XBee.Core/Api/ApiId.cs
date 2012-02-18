namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public enum ApiId
    {
        /// <summary>
        /// API ID: 0x0
        /// </summary>
        TX_REQUEST_64 = 0x0,

        /// <summary>
        /// API ID: 0x1
        /// </summary>
        TX_REQUEST_16 = 0x1,

        /// <summary>
        /// API ID: 0x08
        /// </summary>
        AT_COMMAND = 0x08,

        /// <summary>
        /// API ID: 0x09
        /// </summary>
        AT_COMMAND_QUEUE = 0x09,

        /// <summary>
        /// API ID: 0x17
        /// </summary>
        REMOTE_AT_REQUEST = 0x17,

        /// <summary>
        /// API ID: 0x10
        /// </summary>
        ZNET_TX_REQUEST = 0x10,

        /// <summary>
        /// API ID: 0x11
        /// </summary>
        ZNET_EXPLICIT_TX_REQUEST = 0x11,

        /// <summary>
        /// API ID: 0x80
        /// </summary>
        RX_64_RESPONSE = 0x80,

        /// <summary>
        /// API ID: 0x81
        /// </summary>
        RX_16_RESPONSE = 0x81,

        /// <summary>
        /// API ID: 0x82
        /// </summary>
        RX_64_IO_RESPONSE = 0x82,

        /// <summary>
        /// API ID: 0x83
        /// </summary>
        RX_16_IO_RESPONSE = 0x83,

        /// <summary>
        /// API ID: 0x88
        /// </summary>
        AT_RESPONSE = 0x88,

        /// <summary>
        /// API ID: 0x89
        /// </summary>
        TX_STATUS_RESPONSE = 0x89,

        /// <summary>
        /// API ID: 0x8a
        /// </summary>
        MODEM_STATUS_RESPONSE = 0x8a,

        /// <summary>
        /// API ID: 0x90
        /// </summary>
        ZNET_RX_RESPONSE = 0x90,

        /// <summary>
        /// API ID: 0x91
        /// </summary>
        ZNET_EXPLICIT_RX_RESPONSE = 0x91,

        /// <summary>
        /// API ID: 0x8b
        /// </summary>
        ZNET_TX_STATUS_RESPONSE = 0x8b,

        /// <summary>
        /// API ID: 0x97
        /// </summary>
        REMOTE_AT_RESPONSE = 0x97,

        /// <summary>
        /// API ID: 0x92
        /// </summary>
        ZNET_IO_SAMPLE_RESPONSE = 0x92,

        /// <summary>
        /// API ID: 0x95
        /// </summary>
        ZNET_IO_NODE_IDENTIFIER_RESPONSE = 0x95,

        /// <summary>
        /// Indicates that we've parsed a packet for which we didn't 
        /// know how to handle the API type. This will be parsed 
        /// into a GenericResponse   
        /// </summary>
        UNKNOWN = 0xff,

        /// <summary>
        /// This is returned if an error occurs during packet parsing
        /// and does not correspond to a XBee API ID.
        /// </summary>
        ERROR_RESPONSE = -1
    }
}