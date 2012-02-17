namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// AT commands.
    /// </summary>
    /// <remarks>
    /// Summary descriptions of these commands are copied directly from
    /// Product Manual v1.xEx - 802.15.4 Protocol located at 
    /// http://ftp1.digi.com/support/documentation/90000982_F.pdf
    /// </remarks>
    public enum AtCmd
    {
        #region Special

        /// <summary>
        /// Write. Write parameter values to non-volatile memory so that parameter 
        /// modifications persist through subsequent power-up or reset.
        /// </summary>
        /// <remarks>
        /// Once WR is issued, no additional characters should be sent to the 
        /// module until after the response "OK\r" is received.
        /// </remarks>
        WR,

        /// <summary>
        /// Restore Defaults. Restore module parameters to factory defaults.
        /// </summary>
        RE,

        /// <summary>
        ///  Software Reset. Responds immediately with an OK then performs a 
        ///  hard reset ~100ms later.   
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// </remarks>
        FR,

        #endregion

        #region Networking and Security

        /// <summary>
        /// Channel. Set/Read the channel number used for transmitting and receiving data
        /// between RF modules (uses 802.15.4 protocol channel numbers).  
        /// </summary>
        CH,

        /// <summary>
        /// PAN ID. Set/Read the PAN (Personal Area Network) ID.   
        /// </summary>
        /// <remarks>
        /// Use 0xFFFF to broadcast messages to all PANs. 
        /// </remarks>
        ID,

        /// <summary>
        /// Destination Address High. Set/Read the upper 32 bits of the 64-bit destination
        /// address. 
        /// </summary>
        /// <remarks>
        /// When combined with DL, it defines the destination address used for
        /// transmission. To transmit using a 16-bit address, set DH parameter to zero and DL less
        /// than 0xFFFF.  
        /// </remarks>
        DH,

        /// <summary>
        /// Destination Address Low. Set/Read the lower 32 bits of the 64-bit destination
        /// address.  
        /// </summary>
        /// <remarks>
        /// When combined with DH, DL defines the destination address used for
        /// transmission. To transmit using a 16-bit address, set DH parameter to zero and DL less
        /// than 0xFFFF. 
        /// </remarks>
        DL,

        /// <summary>
        /// 16-bit Source Address. Set/Read the RF module 16-bit source address. 
        /// </summary>
        /// <remarks>
        /// Set MY = 0xFFFF to disable reception of packets with 16-bit addresses.
        /// </remarks>
        MY,

        /// <summary>
        /// Serial Number High. Read high 32 bits of the RF module's unique 
        /// IEEE 64-bit address.   
        /// </summary>
        SH,

        /// <summary>
        /// Serial Number Low. Read low 32 bits of the RF module's unique 
        /// IEEE 64-bit address.    
        /// </summary>
        SL,

        /// <summary>
        ///     
        /// </summary>
        RR,

        /// <summary>
        ///     
        /// </summary>
        RN,

        /// <summary>
        ///     
        /// </summary>
        MM,

        /// <summary>
        /// Node Identifier. Stores a string identifier. The register only accepts printable ASCII 
        /// data. In AT Command Mode, a string can not start with a space. A carriage return ends
        /// the command. Command will automatically end when maximum bytes for the string
        /// have been entered. This string is returned as part of the ND (Node Discover) command.
        /// This identifier is also used with the DN (Destination Node) command. 
        /// </summary>
        NI,

        /// <summary>
        ///     
        /// </summary>
        ND,

        /// <summary>
        ///     
        /// </summary>
        NT,

        /// <summary>
        ///     
        /// </summary>
        NO,

        /// <summary>
        ///     
        /// </summary>
        DN,

        /// <summary>
        ///     
        /// </summary>
        CE,

        /// <summary>
        ///     
        /// </summary>
        SC,

        /// <summary>
        ///     
        /// </summary>
        SD,

        /// <summary>
        ///     
        /// </summary>
        A1,

        /// <summary>
        ///     
        /// </summary>
        A2,

        /// <summary>
        ///     
        /// </summary>
        AI,

        /// <summary>
        ///     
        /// </summary>
        DA,

        /// <summary>
        ///     
        /// </summary>
        FP,

        /// <summary>
        ///     
        /// </summary>
        AS,

        /// <summary>
        ///     
        /// </summary>
        EE,

        /// <summary>
        ///     
        /// </summary>
        KY,

        #endregion

        #region RF Interfacing

        /// <summary>
        ///     
        /// </summary>
        PL,

        /// <summary>
        ///     
        /// </summary>
        CA,

        #endregion

        #region Sleep (Low Power)

        /// <summary>
        ///     
        /// </summary>
        SM,

        /// <summary>
        ///     
        /// </summary>
        SO,

        /// <summary>
        ///     
        /// </summary>
        ST,

        /// <summary>
        ///     
        /// </summary>
        SP,

        /// <summary>
        ///     
        /// </summary>
        DP,

        #endregion

        #region Serial Interfacing

        /// <summary>
        ///     
        /// </summary>
        BD,

        /// <summary>
        ///     
        /// </summary>
        RO,

        /// <summary>
        ///     
        /// </summary>
        AP,

        /// <summary>
        ///     
        /// </summary>
        NB,

        /// <summary>
        ///     
        /// </summary>
        PR,

        #endregion

        #region IO Settings

        /// <summary>
        ///     
        /// </summary>
        D7,

        /// <summary>
        ///     
        /// </summary>
        D6,

        /// <summary>
        ///     
        /// </summary>
        D5,

        /// <summary>
        ///     
        /// </summary>
        D4,

        /// <summary>
        ///     
        /// </summary>
        D3,

        /// <summary>
        ///     
        /// </summary>
        D2,

        /// <summary>
        ///     
        /// </summary>
        D1,

        /// <summary>
        ///     
        /// </summary>
        D0,

        /// <summary>
        ///     
        /// </summary>
        IU,

        /// <summary>
        ///     
        /// </summary>
        IT,

        /// <summary>
        ///     
        /// </summary>
        IS,

        /// <summary>
        ///     
        /// </summary>
        IO,

        /// <summary>
        ///     
        /// </summary>
        IC,

        /// <summary>
        ///     
        /// </summary>
        IR,

        /// <summary>
        ///     
        /// </summary>
        IA,

        /// <summary>
        ///     
        /// </summary>
        T0,

        /// <summary>
        ///     
        /// </summary>
        T1,

        /// <summary>
        ///     
        /// </summary>
        T2,

        /// <summary>
        ///     
        /// </summary>
        T3,

        /// <summary>
        ///     
        /// </summary>
        T4,

        /// <summary>
        ///     
        /// </summary>
        T5,

        /// <summary>
        ///     
        /// </summary>
        T6,

        /// <summary>
        ///     
        /// </summary>
        T7,

        /// <summary>
        ///     
        /// </summary>
        P0,

        /// <summary>
        ///     
        /// </summary>
        P1,

        /// <summary>
        ///     
        /// </summary>
        M0,

        /// <summary>
        ///     
        /// </summary>
        M1,

        /// <summary>
        ///     
        /// </summary>
        PT,

        /// <summary>
        ///     
        /// </summary>
        RP,

        #endregion

        #region Diagnostics

        /// <summary>
        ///     
        /// </summary>
        VR,

        /// <summary>
        ///     
        /// </summary>
        VL,

        /// <summary>
        ///     
        /// </summary>
        HV,

        /// <summary>
        ///     
        /// </summary>
        DB,

        /// <summary>
        ///     
        /// </summary>
        EC,

        /// <summary>
        ///     
        /// </summary>
        EA,

        /// <summary>
        ///     
        /// </summary>
        ED,

        #endregion

        #region AT Command Options

        /// <summary>
        ///     
        /// </summary>
        CT,

        /// <summary>
        ///     
        /// </summary>
        CN,

        /// <summary>
        ///     
        /// </summary>
        AC,

        /// <summary>
        ///     
        /// </summary>
        GT,

        /// <summary>
        ///     
        /// </summary>
        GC

        #endregion
    }
}