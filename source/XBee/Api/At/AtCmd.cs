namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// AT commands.
    /// </summary>
    /// <remarks>
    /// Summary descriptions of these commands are copied from Digi manuals
    /// </remarks>
    public enum AtCmd : ushort
    {
        #region Special

        /// <summary>
        /// AC.
        /// <para>Applies changes to all command registers causing queued command
        /// register values to be applied. For example, changing the serial interface rate with the BD
        /// command will not change the UART interface rate until changes are applied with the AC
        /// command. The CN command and 0x08 API command frame also apply changes.</para>
        /// </summary>
        ApplyChanges = 0x4143,

        /// <summary>
        /// WR. 
        /// <para>Write parameter values to non-volatile memory so that parameter 
        /// modifications persist through subsequent power-up or reset.</para>
        /// </summary>
        /// <remarks>
        /// Once WR is issued, no additional characters should be sent to the 
        /// module until after the response "OK\r" is received.
        /// </remarks>
        Write = 0x5752,

        /// <summary>
        /// RE. 
        /// <para>Restore module parameters to factory defaults.</para>
        /// </summary>
        RestoreDefaults = 0x5245,

        /// <summary>
        /// FR. 
        /// <para>Responds immediately with an OK then performs a 
        /// hard reset ~100ms later.</para>
        /// </summary>
        /// <remarks>
        /// Introduced in firmware v1.x80.
        /// </remarks>
        SoftwareReset = 0x4652,

        /// <summary>
        /// NR.
        /// <para>Reset network layer parameters on one or more modules within a PAN.
        /// Responds immediately with an 'OK' then causes a network restart. All network
        /// configuration and routing information is consequently lost.
        /// If NR = 0: Resets network layer parameters on the node issuing the command.
        /// If NR = 1: Sends broadcast transmission to reset network 
        /// layer parameters on all nodes in the PAN.</para>
        /// </summary>
        NetworkReset = 0x4E52,

        /// <summary>
        /// SI.
        /// <para>Cause a cyclic sleep module to sleep immediately rather than wait
        /// for the ST timer to expire.</para>
        /// </summary>
        SleepImmediately = 0x5349,

        /// <summary>
        /// ND.
        /// <para>Discovers and reports all RF modules found. After (NT * 100) milliseconds, 
        /// the command ends by returning a carrage return. ND also accepts a Node Identifier (NI) as 
        /// a parameter (optional). In this case, only a module that matches the supplied 
        /// identifier will respond. If ND is sent through the API, each response is returned 
        /// as a separate AT_CMD_Response packet. The data consists of the above listed bytes without the
        /// carriage return delimiters. The NI string will end in a "0x00" null character. The radius of
        /// the ND command is set by the BH command.</para>
        /// </summary>
        NodeDiscover = 0x4E44,

        /// <summary>
        /// DN.
        /// <para>Resolves an NI (Node Identifier) string to a physical address (casesensitive).
        /// The 16-bit network and 64-bit extended addresses are returned in an API Command Response frame.
        /// If there is no response from a module within (NT * 100) milliseconds or a parameter is
        /// not specified (left blank), the command is terminated and an 'ERROR' message is
        /// returned. In the case of an ERROR, Command Mode is not exited. The radius of the DN
        /// command is set by the BH command</para>
        /// </summary>
        DestinationNode = 0x444E,

        #endregion

        #region Adressing

        /// <summary>
        /// MY.
        /// <para>16-bit Source Address. Set/Read the RF module 16-bit source address.</para>
        /// </summary>
        /// <remarks>
        /// Set MY = 0xFFFF to disable reception of packets with 16-bit addresses.
        /// </remarks>
        NetworkAddress = 0x4D59,

        /// <summary>
        /// SH.
        /// <para>Serial Number High. Read high 32 bits of the RF module's unique 
        /// IEEE 64-bit address.</para>
        /// </summary>
        SerialNumberHigh = 0x5348,

        /// <summary>
        /// SL.
        /// <para>Serial Number Low. Read low 32 bits of the RF module's unique 
        /// IEEE 64-bit address.</para>
        /// </summary>
        SerialNumberLow = 0x534C,

        /// <summary>
        /// DH.
        /// <para>Destination Address High. Set/Read the upper 32 bits of the 64-bit destination address.</para>
        /// </summary>
        /// <remarks>
        /// When combined with DL, it defines the destination address used for
        /// transmission. To transmit using a 16-bit address, set DH parameter to zero and DL less
        /// than 0xFFFF.  
        /// </remarks>
        DestinationAddressHigh = 0x4448,

        /// <summary>
        /// DL.
        /// <para>Destination Address Low. Set/Read the lower 32 bits of the 64-bit destination address.</para>
        /// </summary>
        /// <remarks>
        /// When combined with DH, DL defines the destination address used for
        /// transmission. To transmit using a 16-bit address, set DH parameter to zero and DL less
        /// than 0xFFFF. 
        /// </remarks>
        DestinationAddressLow = 0x444C,

        /// <summary>
        /// MP.
        /// <para>Read the 16-bit network address of the module's
        /// parent. A value of 0xFFFE means the module does not have a parent.</para>
        /// </summary>
        ParentAddress = 0x4D50,

        /// <summary>
        /// NI.
        /// <para>Stores a string identifier. The register only accepts printable ASCII 
        /// data. In AT Command Mode, a string can not start with a space. A carriage return ends
        /// the command. Command will automatically end when maximum bytes for the string
        /// have been entered. This string is returned as part of the ND (Node Discover) command.
        /// This identifier is also used with the DN (Destination Node) command.</para>
        /// </summary>
        NodeIdentifier = 0x4E49,

        /// <summary>
        /// DD.
        /// <para>Stores a device type value. This value can be used to
        /// differentiate different XBee-based devices. Digi reserves the range 0 - 0xFFFFFF.</para>
        /// </summary>
        DeviceTypeIdentifier = 0x4444,

        #endregion

        #region Networking

        /// <summary>
        /// CH.
        /// <para>Set/Read the channel number used for transmitting 
        /// and receiving data between RF modules</para>
        /// </summary>
        OperatingChannel = 0x4348, 

        /// <summary>
        /// ID. 
        /// <para>Set/Read the PAN (Personal Area Network) ID.</para>
        /// </summary>
        /// <remarks>
        /// Use 0xFFFF to broadcast messages to all PANs. 
        /// </remarks>
        ExtendedPanId = 0x4944,

        /// <summary>
        /// OP.
        /// <para>Read the 64-bit extended PAN ID. The OP value reflects the operating extended 
        /// PAN ID that the module is running on. If ID > 0, OP will equal ID</para>
        /// </summary>
        OperatingExtendedPanId = 0x4F50,

        /// <summary>
        /// NH.
        /// <para>Set / read the maximum hops limit. This limit sets the
        /// maximum broadcast hops value (BH) and determines the unicast timeout. The timeout
        /// is computed as (50 * NH) + 100 ms. The default unicast timeout of 1.6 seconds
        /// (NH=0x1E) is enough time for data and the acknowledgment to traverse about 8 hops.</para>
        /// </summary>
        MaximumUnicastHops = 0x4E48,

        /// <summary>
        /// BH.
        /// <para>Set/Read the maximum number of hops for each broadcast data
        /// transmission. Setting this to 0 will use the maximum number of hops</para>
        /// </summary>
        BroadcastHops = 0x4248,

        /// <summary>
        /// OI.
        /// <para>Read the 16-bit PAN ID. The OI value reflects the actual 16-bit 
        /// PAN ID the module is running on.</para>
        /// </summary>
        OperatingPanId = 0x4F49,

        /// <summary>
        /// NT.
        /// <para>Set/Read the node discovery timeout. When the network
        /// discovery (ND) command is issued, the NT value is included in the transmission to
        /// provide all remote devices with a response timeout. Remote devices wait a random
        /// time, less than NT, before sending their response</para>
        /// </summary>
        NodeDiscoverTimeout = 0x4E54,

        /// <summary>
        /// NO.
        /// <para>Set/Read the options value for the network discovery
        /// command. The options bitfield value can change the behavior of the ND (network
        /// discovery) command and/or change what optional values are returned in any received
        /// ND responses or API node identification frames. Options include:
        /// 0x01 = Append DD value (to ND responses or API node identification frames)
        /// 002 = Local device sends ND response frame when ND is issued.</para>
        /// </summary>
        NetworkDiscoveryOptions = 0x4E4F,

        /// <summary>
        /// SC.
        /// <para>Set/Read bit field list of channels to scan.</para>
        /// </summary>
        ScanChannels = 0x5343,

        /// <summary>
        /// SD.
        /// <para>Set/Read the scan duration exponent. Time equals to (2 ^ SD) * 15.36ms.</para>
        /// </summary>
        ScanDuration = 0x5344,

        /// <summary>
        /// ZS.
        /// <para>Set / read the ZigBee stack profile value. This must be set the
        /// same on all devices that should join the same network.</para>
        /// </summary>
        ZigBeeStackProfile = 0x5A53,

        /// <summary>
        /// NJ.
        /// <para>Set/Read the time that a Coordinator/Router allows nodes to join.
        /// This value can be changed at run time without requiring a Coordinator or Router to
        /// restart. The time starts once the Coordinator or Router has started. The timer is reset
        /// on power-cycle or when NJ changes.</para>
        /// </summary>
        NodeJoinTime = 0x4E4A,

        /// <summary>
        /// JV.
        /// <para>Set/Read the channel verification parameter. If JV=1, a router
        /// will verify the coordinator is on its operating channel when joining or coming up from a
        /// power cycle. If a coordinator is not detected, the router will leave its current channel and
        /// attempt to join a new PAN. If JV=0, the router will continue operating on its current
        /// channel even if a coordinator is not detected.</para>
        /// </summary>
        ChannelVerification = 0x4A56,

        /// <summary>
        /// JN.
        /// <para>Set / read the join notification setting. If enabled, the module will
        /// transmit a broadcast node identification packet on power up and when joining. This
        /// action blinks the Associate LED rapidly on all devices that receive the transmission, and
        /// sends an API frame out the UART of API devices. This feature should be disabled for
        /// large networks to prevent excessive broadcasts.</para>
        /// </summary>
        JoinNotification = 0x4A4E,

        /// <summary>
        /// AR.
        /// <para>Set/read time between consecutive aggregate route
        /// broadcast messages. If used, AR should be set on only one device to enable many-toone
        /// routing to the device. Setting AR to 0 only sends one broadcast</para>
        /// </summary>
        AggregateRoutingNotification = 0x4152,

        /// <summary>
        /// AI.
        /// <para>Read information regarding last node join request.</para>
        /// </summary>
        AssociationIndication = 0x4149,

        /// <summary>
        /// NP.
        /// <para>This value returns the maximum number of RF payload
        /// bytes that can be sent in a unicast transmission. If many-to-one and source routing are
        /// used (AR less then 0xFF), or if APS security is used on a transmission, the maximum payload
        /// size is reduced further.</para>
        /// </summary>
        MaxPayloadBytes = 0x4E50,

        /// <summary>
        /// CE.
        /// <para>Set/Read the coordinator setting.</para>
        /// </summary>
        CoordinatorEnable = 0x4345,

        /// <summary>
        /// AS.
        /// <para>Send Beacon Request to Broadcast Address (0xFFFF) and Broadcast PAN (0xFFFF) on every channel. 
        /// The parameter determines the time the radio will listen for Beacons on each channel. 
        /// A PanDescriptor is created and returned for every Beacon received from the scan. 
        /// The Active Scan is capable of returning up to 5 PanDescriptors in a scan. 
        /// The actual scan time on each channel is measured as Time = [(2 ^SD PARAM) * 15.36] ms. 
        /// Note the total scan time is this time multiplied by the number of channels to be scanned 
        /// (16 for the XBee and 13 for the XBee-PRO).</para>
        /// </summary>
        ActiveScan = 0x4153,

        #endregion

        #region Security

        /// <summary>
        /// EE.
        /// <para>Set/Read the encryption enable setting.</para>
        /// </summary>
        EncryptionEnable = 0x4545,

        /// <summary>
        /// EO.
        /// <para>Options for encryption. Unused option bits should be set to 0.</para>
        /// </summary>
        EncryptionOptions = 0x454F,

        /// <summary>
        /// NK.
        /// <para>Set the 128-bit AES network encryption key. This command
        /// is write-only; NK cannot be read. If set to 0 (default), the module will select a random
        /// network key.</para>
        /// </summary>
        EncryptionKey = 0x4E4B,

        /// <summary>
        /// KY.
        /// <para>Set the 128-bit AES link key. This command is write only; KY cannot be read.
        /// Setting KY to 0 will cause the coordinator to transmit the network key in the clear to
        /// joining devices, and will cause joining devices to acquire the network key in the clear
        /// when joining.</para>
        /// </summary>
        LinkKey = 0x4B59,

        #endregion

        #region RF Interfacing

        /// <summary>
        /// PL.
        /// <para>Select/Read the power level at which the RF module transmits conducted power.</para>
        /// </summary>
        PowerLevel = 0x504C,

        /// <summary>
        /// PM.
        /// <para>Set/read the power mode of the device. Enabling boost mode will improve
        /// the receive sensitivity by 1dB and increase the transmit power by 2dB
        /// Note: Enabling boost mode on the XBee-PRO will not affect the output power. Boost
        /// mode imposes a slight increase in current draw. See section 1.2 for details.</para>
        /// </summary>
        PowerMode = 0x504D,

        /// <summary>
        /// DB.
        /// <para>This command reports the received signal strength of the
        /// last received RF data packet. The DB command only indicates the signal strength of the
        /// last hop. It does not provide an accurate quality measurement for a multihop link. DB
        /// can be set to 0 to clear it. The DB command value is measured in -dBm. For example if
        /// DB returns 0x50, then the RSSI of the last packet received was -80dBm.</para>
        /// </summary>
        ReceivedSignalStrength = 0x4442,

        #endregion

        #region Sleep (Low Power)

        /// <summary>
        /// SM.
        /// <para>Sets the sleep mode on the RF module</para>
        /// </summary>
        SleepMode = 0x534D,

        /// <summary>
        /// SN.
        /// <para>Sets the number of sleep periods to not assert the On/Sleep
        /// pin on wakeup if no RF data is waiting for the end device. This command allows a host
        /// application to sleep for an extended time if no RF data is present</para>
        /// </summary>
        NumberOfSleepPeriods = 0x534E,

        /// <summary>
        /// SO.
        /// <para>Configure options for sleep. Unused option bits should be set to 0.
        /// Sleep options include:
        /// 0x02 - Always wake for ST time
        /// 0x04 - Sleep entire SN * SP time
        /// Sleep options should not be used for most applications. See Sleep Mode chapter for
        /// more information</para>
        /// </summary>
        SleepOptions = 0x534F,

        /// <summary>
        /// ST.
        /// <para>Sets the time before sleep timer on an end device.The timer is reset
        /// each time serial or RF data is received. Once the timer expires, an end device may enter
        /// low power operation. Applicable for cyclic sleep end devices only.</para>
        /// </summary>
        TimeBeforeSleep = 0x5354,

        /// <summary>
        /// SP.
        /// <para>This value determines how long the end device will sleep at a time, up to
        /// 28 seconds. (The sleep time can effectively be extended past 28 seconds using the SN
        /// command.) On the parent, this value determines how long the parent will buffer a
        /// message for the sleeping end device. It should be set at least equal to the longest SP
        /// time of any child end device.</para>
        /// </summary>
        SleepPeriod = 0x534C,

        /// <summary>
        /// WH.
        /// <para>Set/Read the wake host timer value. If the wake host timer is set to a nonzero
        /// value, this timer specifies a time (in millisecond units) that the device should allow
        /// after waking from sleep before sending data out the UART or transmitting an IO sample.
        /// If serial characters are received, the WH timer is stopped immediately.</para>
        /// </summary>
        WakeHost = 0x5748,

        #endregion

        #region Serial Interfacing

        /// <summary>
        /// BD.
        /// <para>Set/Read the serial interface data rate for communication between
        /// the module serial port and host. Any value above 0x07 will be interpreted 
        /// as an actual baud rate. When a value above 0x07 is sent, the closest
        /// interface data rate represented by the number is stored in the BD register.</para>
        /// </summary>
        InterfaceDataRate = 0x4244,

        /// <summary>
        /// RO.
        /// <para>Set/Read number of character times of inter-character delay
        /// required before transmission. Set to zero to transmit characters as 
        /// they arrive instead of buffering them into one RF packet.</para>
        /// </summary>
        PacketizationTimeout = 0x524F,

        /// <summary>
        /// AP.
        /// <para>Disable/Enable API Mode</para>
        /// </summary>
        ApiEnable = 0x4150,

        /// <summary>
        /// NB.
        /// <para>Set/Read parity settings.</para>
        /// </summary>
        /// <remarks>
        /// 0 = 8-bit (no parity) or 7-bit (any parity)
        /// 1 = 8-bit even
        /// 2 = 8-bit odd
        /// 3 = 8-bit mark
        /// 4 = 8-bit space
        /// </remarks>
        SerialParity = 0x4E42,

        #endregion

        #region IO Settings

        /// <summary>
        /// IS.
        /// <para>Forces a read of all enabled digital and analog input lines.</para>
        /// </summary>
        ForceSample = 0x4953,

        /// <summary>
        /// 1S.
        /// <para>Forces a sample to be taken on an XBee Sensor device. This command can 
        /// only be issued to an XBee sensor device using an API remote command.</para>
        /// </summary>
        XbeeSensorSample = 0x3153,

        /// <summary>
        /// IR.
        /// <para>Set/Read the IO sample rate to enable periodic sampling. For periodic
        /// sampling to be enabled, IR must be set to a non-zero value, and at least one module pin
        /// must have analog or digital IO functionality enabled (see D0-D8, P0-P2 commands).
        /// The sample rate is measured in milliseconds.</para>
        /// </summary>
        SampleRate = 0x4952,

        /// <summary>
        /// IC.
        /// <para>Set/Read the digital IO pins to monitor for changes in the
        /// IO state. IC works with the individual pin configuration commands (D0-D8, P0-P2). If a
        /// pin is enabled as a digital input/output, the IC command can be used to force an
        /// immediate IO sample transmission when the DIO state changes. IC is a bitmask that
        /// can be used to enable or disable edge detection on individual channels. Unused bits
        /// should be set to 0. Bit (IO pin): 0 (DIO0)</para>
        /// </summary>
        DigitalChangeDirection = 0x4943,

        /// <summary>
        /// LT.
        /// <para>Set/Read the Associate LED blink time. If the Associate LED
        /// functionality is enabled (D5 command), this value determines the on and off blink times
        /// for the LED when the module has joined a network. If LT=0, the default blink rate will be
        /// used (500ms coordinator, 250ms router/end device). For all other LT values, LT is
        /// measured in 10ms.</para>
        /// </summary>
        AssociateLedBlinkTime = 0x4C54,

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
        /// </remarks>
        PullUpResistor = 0x4C52,

        /// <summary>
        /// RP.
        /// <para>Time RSSI signal will be output after last transmission. When RP =
        /// 0xFF, output will always be on.</para>
        /// </summary>
        RssiPwmTimer = 0x524C,

        /// <summary>
        /// CB.
        /// <para>This command can be used to simulate commissioning
        /// button presses in software. The parameter value should be set to the number of button
        /// presses to be simulated. For example, sending the ATCB1 command will execute the
        /// action associated with 1 commissioning button press</para>
        /// </summary>
        CommissioningPushButton = 0x4342,

        /// <summary>
        /// %V.
        /// <para>Reads the voltage on the Vcc pin. Divide the read value by 1024.</para>
        /// <example>A %V reading of 0x900 (2304 decimal) represents 2700mV or 2.70V.</example>
        /// </summary>
        SupplyVoltage = 0x2556,

        #endregion

        #region Diagnostics

        /// <summary>
        /// VR.
        /// <para>Read firmware version of the module.
        /// The firmware version returns 4 hexadecimal values (2 bytes) "ABCD". Digits ABC are
        /// the main release number and D is the revision number from the main release. "B" is a
        /// variant designator.</para>
        /// </summary>
        FirmwareVersion = 0x5652,

        /// <summary>
        /// HV.
        /// <para>Read the hardware version of the module.version of the module.
        /// This command can be used to distinguish among different hardware platforms. The
        /// upper byte returns a value that is unique to each module type. The lower byte indicates
        /// the hardware revision.</para>
        /// </summary>
        /// <remarks>
        /// XBee ZB and XBee ZNet modules return the following (hexadecimal) values:
        /// 0x19xx - XBee module
        /// 0x1Axx - XBee-PRO module
        /// </remarks>
        HardwareVersion = 0x4856,

        /// <summary>
        /// ED.
        /// <para>Send 'Energy Detect Scan'. ED parameter determines the length of scan
        /// on each channel. The maximal energy on each channel is returned and each value is
        /// followed by a carriage return. Values returned represent detected energy levels in units
        /// of -dBm. Actual scan time on each channel is measured as Time = [(2 ^ SD) * 15.36]
        /// ms. Total scan time is this time multiplied by the number of channels to be scanned.</para>
        /// </summary>
        EnergyScan = 0x4544,

        #endregion

        #region AT Command Options

        /// <summary>
        /// CT.
        /// <para>Set/Read the period of inactivity (no valid commands received) 
        /// after which the RF module automatically exits AT Command Mode and returns
        /// to Idle Mode.</para>
        /// </summary>
        CommandModeTimeout = 0x4354,

        /// <summary>
        /// CN.
        /// <para>Explicitly exit the module from AT Command Mode.</para>
        /// </summary>
        ExitCommandMode = 0x434E,

        /// <summary>
        /// GT.
        /// <para>Set required period of silence before and after the Command Sequence
        /// Characters of the AT Command Mode Sequence (GT + CC + GT). The period of silence
        /// is used to prevent inadvertent entrance into AT Command Mode.</para>
        /// </summary>
        GuardTimes = 0x4754,

        /// <summary>
        /// CC.
        /// <para>Set/Read the ASCII character value to be used
        /// between Guard Times of the AT Command Mode Sequence (GT + CC + GT). The AT
        /// Command Mode Sequence enters the RF module into AT Command Mode.
        /// The CC command is only supported when using AT firmware: 20xx (AT coordinator),
        /// 22xx (AT router), 28xx (AT end device).</para>
        /// </summary>
        CommandSequenceCharacter = 0x4343

        #endregion
    }
}