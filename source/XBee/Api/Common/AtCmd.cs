namespace NETMF.OpenSource.XBee.Api.Common
{
    /// <summary>
    /// Common AT commands.
    /// </summary>
    /// <remarks>
    /// Summary descriptions of these commands are copied from Digi manuals
    /// </remarks>
    public enum AtCmd : ushort
    {
        /// <summary>
        /// SH.
        /// <para>Read the high 32 bits of the module's unique 64-bit address.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 0xFFFFFFFF (read-only).</para>
        /// <para>Default: factory-set.</para>
        /// </remarks>
        SerialNumberHigh = 0x5348,

        /// <summary>
        /// SL.
        /// <para>Read the low 32 bits of the module's unique 64-bit address.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 0xFFFFFFFF ƒ[read-only].</para>
        /// <para>Default: factory-set.</para>
        /// </remarks>
        SerialNumberLow = 0x534C,

        /// <summary>
        /// NI.
        /// <para>Set/Read node string identifier.</para>
        /// </summary>
        NodeIdentifier = 0x4E49,

        /// <summary>
        /// NT.
        /// <para>Set/Read the node discovery timeout.</para>
        /// </summary>
        NodeDiscoverTimeout = 0x4E54,

        /// <summary>
        /// ND.
        /// <para>Discovers and reports all RF modules found.</para>
        /// </summary>
        NodeDiscover = 0x4E44,

        /// <summary>
        /// WR. 
        /// <para>
        /// Write parameter values to non-volatile memory so that parameter 
        /// modifications persist through subsequent power-up or reset.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Once WR is issued, no additional characters should be sent to the 
        /// module until after the response "OK\r" is received.
        /// </remarks>
        Write = 0x5752,

        /// <summary>
        /// VR.
        /// <para>Read firmware version of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 0xFFFF [read-only].</para>
        /// <para>Default: Factory-set.</para>
        /// </remarks>
        FirmwareVersion = 0x5652,

        /// <summary>
        /// HV.
        /// <para>Read hardware version of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0xFFFF [read-only].</para>
        /// <para>Default: Factory-set.</para>
        /// </remarks>
        HardwareVersion = 0x4856,

        /// <summary>
        /// AP.
        /// <para>Disable/Enable API Mode</para>
        /// </summary>
        ApiEnable = 0x4150,

        /// <summary>
        /// IS.
        /// <para>Forces a read of all enabled digital and analog input lines.</para>
        /// </summary>
        ForceSample = 0x4953,

        /// <summary>
        /// MY.
        /// <para>Read the network address of the module.</para>
        /// </summary>
        NetworkAddress = 0x4D59,

        /// <summary>
        /// RE. 
        /// <para>Restore module parameters to factory defaults.</para>
        /// </summary>
        RestoreDefaults = 0x5245,

        /// <summary>
        /// FR. 
        /// <para>Reset module.</para>
        /// </summary>
        SoftwareReset = 0x4652,

        /// <summary>
        /// NR.
        /// <para>Reset network layer parameters.</para>
        /// </summary>
        NetworkReset = 0x4E52,
    }
}