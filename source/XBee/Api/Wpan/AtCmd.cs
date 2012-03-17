namespace NETMF.OpenSource.XBee.Api.Wpan
{
    /// <summary>
    /// AT commands for IEEE® 802.15.4 RF Modules by Digi International.
    /// </summary>
    /// <remarks>
    /// Based on manual from Digi.
    /// <para>Document name: 90000982_F.</para>
    /// <para>Document version: 1/11/2012.</para>
    /// <para>Firwmare version: v1.xED.</para>
    /// </remarks>
    public enum AtCmd : ushort
    {
        #region Special

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
        /// RE. 
        /// <para>
        /// Restore module parameters to factory defaults.
        /// </para>
        /// </summary>
        RestoreDefaults = 0x5245,

        /// <summary>
        /// FR. 
        /// <para>
        /// Responds immediately with an OK then performs 
        /// a hard reset ~100ms later.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// </remarks>
        SoftwareReset = 0x4652,

        #endregion

        #region Adressing

        /// <summary>
        /// CH.
        /// <para>
        /// Set/Read the channel number used for transmitting and 
        /// receiving data between RF modules.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0x0B - 0x1A (XBee), 0x0C - 0x17 (XBee-PRO).</para>
        /// <para>Default: 0x0C (12 dec).</para>
        /// </remarks>
        Channel = 0x4348,

        /// <summary>
        /// ID. 
        /// <para>Set/Read the PAN (Personal Area Network) ID.</para>
        /// </summary>
        /// <remarks>
        /// Use 0xFFFF to broadcast messages to all PANs.
        /// <para>Range: 0 - 0xFFFF.</para>
        /// <para>Default: 0x3332 (13106 dec).</para>
        /// </remarks>
        PanId = 0x4944,

        /// <summary>
        /// DH.
        /// <para>Set/Read the upper 32 bits of the 64-bit destination address.</para>
        /// </summary>
        /// <remarks>
        /// When combined with DL, it defines the destination address used for transmission. 
        /// To transmit using a 16-bit address, set DH parameter to zero and DL less than 0xFFFF.
        /// 0x000000000000FFFF is the broadcast address for the PAN.
        /// <para>Range: 0 - 0xFFFFFFFF.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DestinationAddressHigh = 0x4448,

        /// <summary>
        /// DL.
        /// <para>Set/Read the lower 32 bits of the 64-bit destination address.</para>
        /// </summary>
        /// <remarks>
        /// When combined with DH, DL defines the destination address used for transmission. 
        /// To transmit using a 16-bit address, set DH parameter to zero and DL less than 0xFFFF. 
        /// 0x000000000000FFFF is the broadcast address for the PAN.
        /// <para>Range: 0 - 0xFFFFFFFF.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DestinationAddressLow = 0x444C,

        /// <summary>
        /// MY.
        /// <para>Set/Read the RF module 16-bit source address.</para>
        /// </summary>
        /// <remarks>
        /// Set MY = 0xFFFF to disable reception of packets with 16-bit addresses. 64-bit source 
        /// address (serial number) and broadcast address (0x000000000000FFFF) is always enabled.
        /// <para>Range: 0 - 0xFFFF.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        SourceAddress = 0x4D59,

        /// <summary>
        /// SH.
        /// <para>Read high 32 bits of the RF module's unique IEEE 64-bit address.</para>
        /// </summary>
        /// <remarks>
        /// 64-bit source address is always enabled.
        /// <para>Range: 0 - 0xFFFFFFFF (read-only).</para>
        /// <para>Default: factory-set.</para>
        /// </remarks>
        SerialNumberHigh = 0x5348,

        /// <summary>
        /// SL.
        /// <para>Read low 32 bits of the RF module's unique IEEE 64-bit address.</para>
        /// </summary>
        /// <remarks>
        /// 64-bit source address is always enabled.
        /// <para>Range: 0 - 0xFFFFFFFF (read-only).</para>
        /// <para>Default: factory-set.</para>
        /// </remarks>
        SerialNumberLow = 0x534C,

        /// <summary>
        /// RR.
        /// <para>
        /// Set/Read the maximum number of retries the module will execute in
        /// addition to the 3 retries provided by the 802.15.4 MAC.
        /// </para>
        /// </summary>
        /// <remarks>
        /// For each XBee retry, the 802.15.4 MAC can execute up to 3 retries.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 6.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        XBeeRetries = 0x5252,

        /// <summary>
        /// RN.
        /// <para>
        /// Set/Read the minimum value of the back-off exponent in the CSMA-CA 
        /// algorithm that is used for collision avoidance.
        /// </para>
        /// </summary>
        /// <remarks>
        /// If RN = 0, collision avoidance is disabled during the first iteration 
        /// of the algorithm (802.15.4 - macMinBE).
        /// <para>Range: 0 - 3 (exponent).</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        RandomDelaySlots = 0x524E,

        /// <summary>
        /// MM.
        /// <para>
        /// Set/Read MAC Mode value. MAC Mode enables/disables the
        /// use of a Digi header in the 802.15.4 RF packet.
        /// </para>
        /// </summary>
        /// <remarks>
        /// When Modes 1 or 3 are enabled (MM=1,3), duplicate packet detection is enabled 
        /// as well as certain AT commands.Please see the detailed MM description 
        /// on page 47 for additional information. Introduced in firmware v1.x80.
        /// <para>Range: 
        /// 0 (Digi Mode), 
        /// 1 (802.15.4 no ACKs), 
        /// 2 (802.15.4 with ACKs), 
        /// 3 = (Digi Mode no ACKs)
        /// </para>
        /// <para>Default: 0</para>
        /// </remarks>
        MacMode = 0x4D4D,

        /// <summary>
        /// NI.
        /// <para>Set/Read node string identifier.</para>
        /// </summary>
        /// <remarks>
        /// The register only accepts printable ASCII data. In AT Command Mode, a string 
        /// can not start with a space. A carriage return ends the command. Command will 
        /// automatically end when maximum bytes for the string have been entered. 
        /// This string is returned as part of the <see cref="NodeDiscover"/> command. 
        /// This identifier is also used with the <see cref="DestinationNode"/> command. 
        /// Introduced in firmware v1.x80.
        /// <para>Range: 20-character ASCII string.</para>
        /// <para>Default: empty.</para>
        /// </remarks>
        NodeIdentifier = 0x4E49,

        /// <summary>
        /// ND.
        /// <para>Discover and report all RF modules found.</para>
        /// </summary>
        /// <remarks>
        /// After (<see cref="NodeDiscoverTime"/> * 100) milliseconds, the command ends 
        /// by returning a carrage return. <see cref="NodeDiscover"/> also accepts a 
        /// <see cref="NodeIdentifier"/> as a parameter (optional). In this case, only a module 
        /// that matches the supplied identifier will respond. If <see cref="NodeDiscover"/> 
        /// is sent through the API, each response is returned as a separate response. 
        /// The data consists of the above listed bytes without the carriage return delimiters. 
        /// The <see cref="NodeIdentifier"/> string will end in a "0x00" null character. 
        /// The radius of the <see cref="NodeDiscover"/> command is set by the <see cref="BroadcastHops"/> command. 
        /// Introduced in firmware v1.x80.
        /// <para>Range: optional 20-character NI value.</para>
        /// </remarks>
        NodeDiscover = 0x4E44,

        /// <summary>
        /// NT.
        /// <para>Set/Read the node discovery timeout.</para>
        /// </summary>
        /// <remarks>
        /// When the network discovery (<see cref="NodeDiscover"/>) command is issued, 
        /// the <see cref="NodeDiscoverTime"/> value is included in the transmission to
        /// provide all remote devices with a response timeout. Remote devices wait a random
        /// time, less than NT, before sending their response. Introduced in firmware v1.xA0.
        /// <para>Range: 0x01 - 0xFC (x 100 ms).</para>
        /// <para>Default: 0x19 (25 dec).</para>
        /// </remarks>
        NodeDiscoverTime = 0x4E54,

        /// <summary>
        /// NO.
        /// <para>Enables node discover self-response on the module.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1xC5.
        /// <para>Range: 0 - 1.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        NetworkDiscoveryOptions = 0x4E4F,

        /// <summary>
        /// DN.
        /// <para>Resolves an <see cref="NodeIdentifier"/> string to a physical address.</para>
        /// </summary>
        /// <remarks>
        /// The following events occur upon successful command execution. <see cref="DestinationAddressHigh"/> 
        /// and <see cref="DestinationAddressLow"/> are set to the address of the module with 
        /// the matching <see cref="NodeIdentifier"/>. "OK" is returned. RF module automatically exits 
        /// AT Command Mode. If there is no response from a module within 200 msec or a parameter is not specified
        /// (left blank), the command is terminated and an "ERROR" message is returned. 
        /// Introduced in firmware v1.x80.
        /// <para>Range: 20-character ASCII string.</para>
        /// <para>Default: empty.</para>
        /// </remarks>
        DestinationNode = 0x444E,

        /// <summary>
        /// CE.
        /// <para>Set/Read the coordinator setting.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0 (End Device), 1 (Coordinator).</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        CoordinatorEnable = 0x4345,

        /// <summary>
        /// SC.
        /// <para>Set/Read list of channels to scan for all Active and Energy Scans as a bitfield.</para>
        /// </summary>
        /// <remarks>
        /// This affects scans initiated in command mode (AS, ED) and during End Device Association 
        /// and Coordinator startup. Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0xFFFF [bitfield] (bits 0, 14, 15 not allowed on the XBee-PRO)</para>
        /// <para>Default: 0x1FFE (all XBee-PRO Channels)</para>
        /// </remarks>
        ScanChannels = 0x5343,

        /// <summary>
        /// SD.
        /// <para>Set/Read the scan duration exponent.</para>
        /// </summary>
        /// <remarks>
        /// Time equals to (2 ^ SD) * 15.36ms. Introduced in firmware v1.x80.
        /// <para>End Device - Duration of Active Scan during Association.</para>
        /// <para>Coordinator - If ReassignPANID option is set on Coordinator (refer to A2 parameter),
        /// SD determines the length of time the Coordinator will scan channels to locate existing
        /// PANs. If ReassignChannel option is set, SD determines how long the Coordinator will
        /// perform an Energy Scan to determine which channel it will operate on. Scan Time is measured 
        /// as (# of channels to scan] * (2 ^ SD) * 15.36ms). The number of channels to scan is set 
        /// by the <see cref="ScanChannels"/> command. The XBee can scan up to 16 channels. 
        /// The XBee PRO can scan up to 13 channels.</para>
        /// <para>Range: 0 - 0x0F [exponent].</para>
        /// <para>Default: 4.</para>
        /// </remarks>
        /// <example>
        /// The values below show results for a 13 channel scan:
        /// <list type="bullet">
        /// <item><term>SD = 0</term><description>time = 0.18 sec</description></item>
        /// <item><term>SD = 2</term><description>time = 0.74 sec</description></item>
        /// <item><term>SD = 4</term><description>time = 2.95 sec</description></item>
        /// <item><term>SD = 6</term><description>time = 11.80 sec</description></item>
        /// <item><term>SD = 8</term><description>time = 47.19 sec</description></item>
        /// <item><term>SD = 10</term><description>time = 3.15 min</description></item>
        /// <item><term>SD = 12</term><description>time = 12.58 min</description></item>
        /// <item><term>SD = 14</term><description>time = 50.33 min</description></item>
        /// </list>
        /// </example>
        ScanDuration = 0x5344,

        /// <summary>
        /// A1.
        /// <para>Set/Read End Device association options.</para>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// <list type="bullet">
        /// <item>
        /// <term>bit 0 - ReassignPanIDƒ</term>
        /// <description>
        /// 0 - Will only associate with Coordinator operating on PAN ID that matches module IDƒ. 
        /// 1 - May associate with Coordinator operating on any PAN ID.
        /// </description>
        /// </item>
        /// <item>
        /// <term>bit 1 - ReassignChannel</term>
        /// <description>
        /// 0 - Will only associate with Coordinator operating on matching CH Channel settingƒn. 
        /// 1 - May associate with Coordinator operating on any Channel.
        /// </description>
        /// </item>
        /// <item>
        /// <term>bit 2 - AutoAssociateƒ</term>
        /// <description>
        /// 0 - Device will not attempt Association. 
        /// 1 - Device attempts Association until success. 
        /// This bit is used only for Non-Beacon systems. 
        /// End Devices in Beacon-enabled system must always associate to a Coordinator.
        /// </description>
        /// </item>
        /// <item>
        /// <term>bit 3 - PollCoordOnPinWake</term>
        /// <description>
        /// 0 - Pin Wake will not poll the Coordinator for indirect (pending) data. 
        /// 1 - Pin Wake will send Poll Request to Coordinator to extract any pending data bits 4 - 7 are reserved.
        /// </description>
        /// </item>
        /// </list>
        /// <para>Range: 0 - 0x0F [bitfield].</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        /// </summary>
        EndDeviceAssociation = 0x4131,

        /// <summary>
        /// A2.
        /// <para>Set/Read Coordinator association options.</para>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// <list type="bullet">
        /// <item>
        /// <term>bit 0 - ReassignPanIDƒ</term>
        /// <description>
        /// 0 - Coordinator will not perform Active Scan to locate available PAN ID. It will operate on ID (PAN ID). 
        /// 1 - Coordinator will perform Active Scan to determine an available ID (PAN ID). If a PAN ID conflict 
        /// is found, the ID parameter will change.
        /// </description>
        /// </item>
        /// <item>
        /// <term>bit 1 - ReassignChannel</term>
        /// <description>
        /// 0 - Coordinator will not perform Energy Scan to determine free channel. It will operate on the channel 
        /// determined by the <see cref="Channel"/> parameter. 
        /// 1 - Coordinator will perform Energy Scan to find a free channel, then operate on that channel.
        /// </description>
        /// </item>
        /// <item>
        /// <term>bit 2 - AllowAssociationƒ</term>
        /// <description>
        /// 0 - Coordinator will not allow any devices to associate to it. 
        /// 1 - Coordinator will allow devices to associate to it.
        /// </description>
        /// </item>
        /// <item>
        /// <term>bits 3 - 7</term>
        /// <description>Reserved.</description>
        /// </item>
        /// </list>
        /// <para>Range: 0 - 7 [bitfield].</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        /// </summary>
        CoordinatorAssociation = 0x4132,

        /// <summary>
        /// AI.
        /// <para>Read errors with the last association request.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0x13 [read-only].</para>
        /// </remarks>
        /// <returns><see cref="AssociationStatus"/></returns>
        AssociationIndication = 0x4149,

        /// <summary>
        /// DA.
        /// <para>End Device will immediately disassociate from a Coordinator 
        /// (if associated) and reattempt to associate.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// </remarks>
        ForceDisassociation = 0x4441,

        /// <summary>
        /// FP.
        /// <para>Request indirect messages being held by a coordinator.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80
        /// </remarks>
        ForcePool = 0x4650,

        /// <summary>
        /// AS.
        /// <para>Send Beacon Request to Broadcast Address (0xFFFF) and Broadcast PAN (0xFFFF) on every channel.</para>
        /// </summary>
        /// <remarks>
        /// The parameter determines the time the radio will listen for Beacons on each channel. 
        /// A PanDescriptor is created and returned for every Beacon received from the scan. 
        /// The Active Scan is capable of returning up to 5 PanDescriptors in a scan. 
        /// The actual scan time on each channel is measured as Time = [(2 ^SD PARAM) * 15.36] ms. 
        /// Note the total scan time is this time multiplied by the number of channels to be scanned 
        /// (16 for the XBee and 13 for the XBee-PRO). Also refer to <see cref="ScanDuration"/> command description. 
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0 - 6.</para>
        /// </remarks>
        ActiveScan = 0x4153,

        /// <summary>
        /// ED.
        /// <para>Send an Energy Detect Scan.</para>
        /// </summary>
        /// <remarks>
        /// The parameter determines the length of scan on each channel. The maximal energy on each channel 
        /// is returned and each value is followed by a carriage return. Values returned represent detected 
        /// energy levels in units of -dBm. Actual scan time on each channel is measured as Time = [(2 ^ SD) * 15.36] ms. 
        /// Total scan time is this time multiplied by the number of channels to be scanned. Introduced in firmware v1.x80
        /// <para>Range: 0 - 6.</para>
        /// </remarks>
        EnergyScan = 0x4544,

        /// <summary>
        /// EE.
        /// <para>
        /// Disable/Enable 128-bit AES encryption support. 
        /// Use in conjunction with the <see cref="AesEncryptionKey"/> command.
        /// </para>
        /// <remarks>
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 1.</para>
        /// <para>Default: 0 (disabled).</para>
        /// </remarks>
        AesEncryptionEnable = 0x4545,

        /// <summary>
        /// KY.
        /// <para>
        /// Set the 128-bit AES (Advanced Encryption Standard) key for encrypting/decrypting data. 
        /// The <see cref="AesEncryptionKey"/> register cannot be read.
        /// </para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.xA0
        /// <para>Range: 0 - (any 16-Byte value).</para>
        /// </remarks>
        AesEncryptionKey = 0x4B59,

        #endregion

        #region RF Interfacing

        /// <summary>
        /// PL.
        /// <para>Select/Read the power level at which the RF module transmits conducted power.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 4.</para>
        /// <para>Default: 4.</para>
        /// </remarks>
        /// <returns><see cref="PowerLevel"/></returns>
        PowerLevel = 0x504C,

        /// <summary>
        /// CA.
        /// <para>Set/read the CCA (Clear Channel Assessment) threshold.</para>
        /// </summary>
        /// <remarks>
        /// Prior to transmitting a packet, a CCA is performed to detect energy on the channel. 
        /// If the detected energy is above the CCA Threshold, the module will not transmit the packet.
        /// Introduced in firmware v1.x80
        /// <para>Range: 0x24 - 0x50 [-dBm].</para>
        /// <para>Default: 0x2C (-44 dBm).</para>
        /// </remarks>
        CcaThreshold = 0x4341,

        #endregion

        #region Sleep (Low Power)

        /// <summary>
        /// SM.
        /// <para>Set/Read Sleep Mode configurations.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 5.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        /// <returns><see cref="SleepMode"/></returns>
        SleepMode = 0x534D,

        /// <summary>
        /// SO.
        /// <para>Set/Read the sleep mode options.</para>
        /// </summary>
        /// <remarks>
        /// <list type="bullet">
        /// <item><term>Bit 0 - Poll wakeup disable.</term>
        /// <description>
        /// 0 - Normal operations. A module configured for cyclic sleep will poll for data on waking. 
        /// 1 - Disable wakeup poll. A module configured for cyclic sleep will not poll for data on waking.
        /// </description></item>
        /// <item><term>Bit 1 - ADC/DIO wakeup sampling disable.</term>
        /// <description>
        /// 0 - Normal operations. A module configured in a sleep mode with ADC/DIO sampling enabled will 
        /// automatically perform a sampling on wakeup. 1 - Suppress sample on wakeup. A module configured 
        /// in a sleep mode with ADC/DIO sampling enabled will not automatically sample on wakeup.
        /// </description></item>
        /// </list>
        /// <para>Range: 0 - 4.</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        SleepOptions = 0x534F,

        /// <summary>
        /// ST.
        /// <para>
        /// Set/Read time period of inactivity (no serial or RF data 
        /// is sent or received) before activating Sleep Mode
        /// </para>
        /// </summary>
        /// <remarks>
        /// ST parameter is only valid with Cyclic Sleep settings (<see cref="SleepMode"/> = 4 - 5).
        /// Coordinator and End Device ST values must be equal.
        /// Also note, the GT parameter value must always be less than the ST value. (If GT > ST,
        /// the configuration will render the module unable to enter into command mode.) 
        /// If the ST parameter is modified, also modify the GT parameter accordingly.
        /// <para>Range: 1 - 0xFFFF [x 1 ms].</para>
        /// <para>Default: 0x1388 (5000 dec).</para>
        /// </remarks>
        TimeBeforeSleep = 0x5354,

        /// <summary>
        /// SP.
        /// <para>Set/Read sleep period for cyclic sleeping remotes</para>
        /// </summary>
        /// <remarks>
        /// Coordinator and End Device SP values should always be equal. 
        /// To send Direct Messages, set SP = 0.
        /// <para>
        /// End Device - SP determines the sleep period for cyclic sleeping remotes. 
        /// Maximum sleep period is 268 seconds (0x68B0).
        /// </para>
        /// <para>
        /// Coordinator - If non-zero, SP determines the time to hold an indirect message before
        /// discarding it. A Coordinator will discard indirect messages after a period of (2.5 * SP).
        /// </para>
        /// <para>Range: 0 - 0x68B0 [x 10 ms].</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        CyclicSleepPeriod = 0x534C,

        /// <summary>
        /// DP.
        /// <para>Set/Read time period of sleep for cyclic sleeping remotes that are 
        /// configured for Association but are not associated to a Coordinator.</para>
        /// </summary>
        /// <remarks>
        /// If a device is configured to associate, configured as a Cyclic Sleep remote, 
        /// but does not find a Coordinator, it will sleep for DP time before reattempting association. 
        /// Maximum sleepperiod is 268 seconds (0x68B0). DP should be > 0 for NonBeacon systems.
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0x68B0 [x 10 ms].</para>
        /// <para>Default: 0x3E8 (1000 dec).</para>
        /// </remarks>
        DisassociatedCyclicSleepPeriod = 0x4450,

        #endregion

        #region Serial Interfacing

        /// <summary>
        /// BD.
        /// <para>Set/Read the serial interface data rate for communication between
        /// the module serial port and host.</para>
        /// </summary>
        /// <remarks>
        /// Request non-standard baud rates with values above 0x80 using a terminal window. 
        /// Read the BD register to find actual baud rate achieved.
        /// <para>Range: 
        /// 0 - 7 (standard baud rates), 
        /// 0x80 (non-standard baud rates up to 250 Kbps).
        /// </para>
        /// <para>Default: 3 (9600 Kbps).</para>
        /// </remarks>
        InterfaceDataRate = 0x4244,

        /// <summary>
        /// RO.
        /// <para>Set/Read number of character times of inter-character delay required before transmission.</para>
        /// </summary>
        /// <remarks>
        /// Set to zero to transmit characters as they arrive instead of buffering them into one RF packet.
        /// <para>Range: 0 - 0xFF [x character times]</para>
        /// <para>Default: 3.</para>
        /// </remarks>
        PacketizationTimeout = 0x524F,

        /// <summary>
        /// AP.
        /// <para>Disable/Enable API Mode</para>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0-2.</para>
        /// <para>Default: 0 (Disabled).</para>
        /// </remarks>
        /// </summary>
        ApiEnable = 0x4150,

        /// <summary>
        /// NB.
        /// <para>Set/Read parity settings.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 4.</para>
        /// <para>Default: 0 (8-bit no parity).</para>
        /// </remarks>
        Parity = 0x4E42,

        /// <summary>
        /// PR.
        /// <para>Set/Read bitfield to configure internal pull-up resistor status for I/O lines</para>
        /// </summary>
        /// <remarks>
        /// Bit set to '1' specifies pull-up enabled, '0' specifies no pull-up.
        /// Bitfield Map:
        /// bit 0 - AD4/DIO4 (pin11)
        /// bit 1 - AD3 / DIO3 (pin17)
        /// bit 2 - AD2/DIO2 (pin18)
        /// bit 3 - AD1/DIO1 (pin19)
        /// bit 4 - AD0 / DIO0 (pin20)
        /// bit 5 - RTS / AD6 / DIO6 (pin16)
        /// bit 6 - DTR / SLEEP_RQ / DI8 (pin9)
        /// bit 7 - DIN/CONFIG (pin3)
        /// Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        PullUpResistorEnable = 0x4C52,

        #endregion

        #region IO Settings

        /// <summary>
        /// D8.
        /// <para>Select/Read options for the DI8 line (pin 9) of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 (Disabled), 3 (DI).</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO8Config = 0x4438,

        /// <summary>
        /// D7.
        /// <para>Select/Read settings for the DIO7 line (pin 12) of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// Options include CTS flow control and I/O line settings.
        /// Introduced in firmware v1.x80.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (CTS Flow Controlƒ), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high), 
        /// 6 (RS485 Tx Enable Low), 
        /// 7 (RS485 Tx Enable High).
        /// </para>
        /// <para>Default: 1.</para>
        /// </remarks>
        DIO7Config = 0x4437,

        /// <summary>
        /// D6.
        /// <para>Select/Read settings for the DIO6 line (pin 16) of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// Options include RTS flow control and I/O line settings.
        /// Introduced in firmware v1.x80.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (RTS Flow Controlƒ), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO6Config = 0x4436,

        /// <summary>
        /// D5.
        /// <para>Configure settings for the DIO5 line (pin 15) of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// Options include Associated LED indicator (blinks when associated) and I/O line settings.
        /// Introduced in firmware v1.x80.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (Associated indicatorƒ), 
        /// 2 (ADC), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 1.</para>
        /// </remarks>
        DIO5Config = 0x4435,

        /// <summary>
        /// D4.
        /// <para>Select/Read settings for the AD4/DIO4 (pin 11).</para>
        /// </summary>
        /// <remarks>
        /// Options include: Analog-to-digital converter, Digital Input and Digital Output.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (ADC), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO4Config = 0x4434,

        /// <summary>
        /// D3.
        /// <para>Select/Read settings for the AD3/DIO3 (pin 17).</para>
        /// </summary>
        /// <remarks>
        /// Options include: Analog-to-digital converter, Digital Input and Digital Output.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (ADC), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO3Config = 0x4433,

        /// <summary>
        /// D2.
        /// <para>Select/Read settings for the AD2/DIO2 (pin 18).</para>
        /// </summary>
        /// <remarks>
        /// Options include: Analog-to-digital converter, Digital Input and Digital Output.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (ADC), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO2Config = 0x4432,

        /// <summary>
        /// D1.
        /// <para>Select/Read settings for the AD1/DIO1 (pin 19).</para>
        /// </summary>
        /// <remarks>
        /// Options include: Analog-to-digital converter, Digital Input and Digital Output.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (ADC), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO1Config = 0x4431,

        /// <summary>
        /// D0.
        /// <para>Select/Read settings for the AD0/DIO0 (pin 20).</para>
        /// </summary>
        /// <remarks>
        /// Options include: Analog-to-digital converter, Digital Input and Digital Output.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (ADC), 
        /// 3 (DI), 
        /// 4 (DO low), 
        /// 5 (DO high).
        /// </para>
        /// <para>Default: 0.</para>
        /// </remarks>
        DIO0Config = 0x4430,

        /// <summary>
        /// IU.
        /// <para>Disables/Enables I/O data received to be sent out UART.</para>
        /// </summary>
        /// <remarks>
        /// The data is sent using an API frame regardless of the current AP parameter value.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 (Disabled), 1 (Enabled).</para>
        /// <para>Default: 1.</para>
        /// </remarks>
        IOOutputEnable = 0x4955,

        /// <summary>
        /// IT.
        /// <para>Set/Read the number of samples to collect before transmitting data.</para>
        /// </summary>
        /// <remarks>
        /// Maximum number of samples is dependent upon the number of enabled inputs.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 1 - 0xFF.</para>
        /// <para>Default: 1.</para>
        /// </remarks>
        SamplesBeforeTx = 0x4954,

        /// <summary>
        /// IS.
        /// <para>Force a read of all enabled inputs (DI or ADC). Data is returned through the UART.</para>
        /// </summary>
        /// <remarks>
        /// If no inputs are defined (DI or ADC), this command will return error.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 8-bit map (each bit represents the level of an I/O line setup as an output).</para>
        /// </remarks>
        ForceSample = 0x4953,

        /// <summary>
        /// IO.
        /// <para>Set digital output level to allow DIO lines that are setup as 
        /// outputs to be changed through Command Mode.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.xA0.
        /// </remarks>
        DigitalOutputLevel = 0x494F,

        /// <summary>
        /// IC.
        /// <para>Set/Read bitfield values for change detect monitoring.</para>
        /// </summary>
        /// <remarks>
        /// Each bit enables monitoring of DIO0 - DIO7 for changes. If detected, data is transmitted with 
        /// DIO data only. Any samples queued waiting for transmission will be sent first.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [bitfield].</para>
        /// <para>Default: 0 (disabled).</para>
        /// </remarks>
        DIOChangeDetect = 0x4943,

        /// <summary>
        /// IR.
        /// <para>Set/Read sample rate.</para>
        /// </summary>
        /// <remarks>
        /// When set, this parameter causes the module to sample all enabled inputs at a specified interval.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFFFF [x 1 msec].</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        SampleRate = 0x4952,

        #endregion

        #region I/O Line Passing

        /// <summary>
        /// IA.
        /// <para>Set/Read addresses of module to which outputs are bound.</para>
        /// </summary>
        /// <remarks>
        /// Setting all bytes to 0xFF will not allow any received I/O packet to change outputs. 
        /// Setting address to 0xFFFF will allow any received I/O packet to change outputs.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFFFFFFFFFFFFFFFF.</para>
        /// <para>Default: 0xFFFFFFFFFFFFFFFF.</para>
        /// </remarks>
        IOInputAddress = 0x4941,

        /// <summary>
        /// T0.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO0OutputTimeout = 0x5430,

        /// <summary>
        /// T1.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO1OutputTimeout = 0x5431,

        /// <summary>
        /// T2.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO2OutputTimeout = 0x5432,

        /// <summary>
        /// T3.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO3OutputTimeout = 0x5433,

        /// <summary>
        /// T4.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO4OutputTimeout = 0x5434,

        /// <summary>
        /// T0.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO5OutputTimeout = 0x5435,

        /// <summary>
        /// T6.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO6OutputTimeout = 0x5436,

        /// <summary>
        /// T7.
        /// <para>Set/Read Output timeout values for corresponding digital line.</para>
        /// </summary>
        /// <remarks>
        /// When output is set (due to I/O line passing) to a nondefault level, a timer is started which when 
        /// expired will set the output to it default level. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        DIO7OutputTimeout = 0x5437,

        /// <summary>
        /// P0.
        /// <para>Select/Read function for PWM0 pin.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 (Disabled), 1 (RSSI), 2 (PWM Output).</para>
        /// <para>Default: 1.</para>
        /// </remarks>
        PWM0Config = 0x5130,

        /// <summary>
        /// P1.
        /// <para>Select/Read function for PWM0 pin.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 (Disabled), 1 (RSSI), 2 (PWM Output).</para>
        /// <para>Default: 0.</para>
        /// </remarks>
        PWM1Config = 0x5131,

        /// <summary>
        /// M0.
        /// <para>Set/Read the PWM 0 output level.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0x03FF.</para>
        /// </remarks>
        PWM0OutputLevel = 0x4D30,

        /// <summary>
        /// M1.
        /// <para>Set/Read the PWM 1 output level.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0x03FF.</para>
        /// </remarks>
        PWM1OutputLevel = 0x4D31,

        /// <summary>
        /// PT.
        /// <para>Set/Read output timeout value for both PWM outputs.</para>
        /// </summary>
        /// <remarks>
        /// When PWM is set to a non-zero value: Due to I/O line passing, a time is started which when 
        /// expired will set the PWM output to zero. The timer is reset when a valid I/O packet is received.
        /// Introduced in firmware v1.xA0.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0xFF.</para>
        /// </remarks>
        PWMOutputTimeout = 0x5154,

        /// <summary>
        /// RP.
        /// <para>Set/Read PWM timer register.</para>
        /// </summary>
        /// <remarks>
        /// Set the duration of PWM (pulse width modulation) signal output on the RSSI pin. 
        /// The signal duty cycle is updated with each received packet and is shut off when the timer expires.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0x28 (40 dec).</para>
        /// </remarks>
        RssiPwmTimer = 0x5251,

        #endregion

        #region Diagnostics

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
        /// VL.
        /// <para>Read detailed version information (including application build date, MAC, PHY and bootloader versions).</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80. It has been deprecated in version 10C9. 
        /// It is not supported in firmware versions after 10C8.
        /// </remarks>
        FirmwareVersionVerbose = 0x564C,

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
        /// DB.
        /// <para>Read signal level [in dB] of last good packet received (RSSI).</para>
        /// </summary>
        /// <remarks>
        /// Absolute value is reported. (For example: 0x58 = -88 dBm) Reported value 
        /// is accurate between -40 dBm and RX sensitivity.
        /// <para>Range: 0x17-0x5C (XBee), 0x24-0x64 (XBee-PRO) [read-only].</para>
        /// </remarks>
        ReceivedSignalStrength = 0x4442,

        /// <summary>
        /// EC.
        /// <para>Reset/Read count of CCA (Clear Channel Assessment) failures.</para>
        /// </summary>
        /// <remarks>
        /// This parameter value increments when the module does not transmit a packet because it
        /// detected energy above the CCA threshold level set with CA command. This count saturates 
        /// at its maximum value. Set count to "0" to reset count. Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0xFFFF.</para>
        /// </remarks>
        CcaFailures = 0x4543,

        /// <summary>
        /// EC.
        /// <para>Reset/Read count of acknowledgment failures.</para>
        /// </summary>
        /// <remarks>
        /// This parameter value increments when the module expires its transmission retries without 
        /// receiving an ACK on a packet transmission. This count saturates at its maximum value. 
        /// Set the parameter to "0" to reset count. Introduced in firmware v1.x80.
        /// <para>Range: 0 - 0xFFFF.</para>
        /// </remarks>
        AckFailures = 0x4143,

        //*********************************************
        // ED command is duplicated in Adressing region
        //*********************************************

        #endregion

        #region AT Command Options

        /// <summary>
        /// CT.
        /// <para>Set/Read the period of inactivity (no valid commands received) 
        /// after which the RF module automatically exits AT Command Mode and returns
        /// to Idle Mode.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 2 - 0xFFFF [x 100 ms].</para>
        /// <para>Default: 0x64 (100 dec).</para>
        /// </remarks>
        CommandModeTimeout = 0x4354,

        /// <summary>
        /// CN.
        /// <para>Explicitly exit the module from AT Command Mode.</para>
        /// </summary>
        ExitCommandMode = 0x434E,

        /// <summary>
        /// AC.
        /// <para>Explicitly apply changes to queued parameter value(s) and reinitialize module.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.xA0.
        /// </remarks>
        ApplyChanges = 0x4143,

        /// <summary>
        /// GT.
        /// <para>Set required period of silence before and after the Command Sequence
        /// Characters of the AT Command Mode Sequence (GT+ CC + GT). The period of silence
        /// is used to prevent inadvertent entrance into AT Command Mode</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 2 - 0x0CE4 [x 1 ms].</para>
        /// <para>Default: 0x3E8 (1000 dec).</para>
        /// </remarks>
        GuardTimes = 0x4754,

        /// <summary>
        /// CC.
        /// <para>Set/Read the ASCII character value to be used between Guard Times of the AT Command Mode Sequence (GT+CC+GT).</para>
        /// </summary>
        /// <remarks>
        /// The AT Command Mode Sequence enters the RF module into AT Command Mode.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0x2B ('+' in ASCII).</para>
        /// </remarks>
        CommandSequenceCharacter = 0x4343,

        #endregion
    }
}