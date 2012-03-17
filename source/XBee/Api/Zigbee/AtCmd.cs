namespace NETMF.OpenSource.XBee.Api.Zigbee
{
    /// <summary>
    /// AT commands for ZigBee RF Modules by Digi International.
    /// </summary>
    /// <remarks>
    /// Based on manual from Digi.
    /// <para>Document name: 90000976_J.</para>
    /// <para>Document version: January 2012.</para>
    /// <para>Firwmare version: 2x7x.</para>
    /// </remarks>
    public enum AtCmd : ushort
    {
        #region Adressing

        /// <summary>
        /// DH.
        /// <para>Set/Read the upper 32 bits of the 64-bit destination address.</para>
        /// </summary>
        /// <remarks>
        /// When combined with DL, it defines the destination address used for transmission. 
        /// To transmit using a 16-bit address, set DH parameter to zero and DL less than 0xFFFF. 
        /// Special definitions for DH and DL include 0x000000000000FFFF (broadcast) and 
        /// 0x0000000000000000 (coordinator).
        /// <para>Range: 0 - 0xFFFFFFFF.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DestinationAddressHigh = 0x4448,

        /// <summary>
        /// DL.
        /// <para>Set/Read the lower 32 bits of the 64-bit destination address.</para>
        /// </summary>
        /// <remarks>
        /// When combined with DH, DL defines the destination address used for transmission. 
        /// To transmit using a 16-bit address, set DH parameter to zero and DL less than 0xFFFF. 
        /// Special definitions for DH and DL include 0x000000000000FFFF (broadcast) and 
        /// 0x0000000000000000 (coordinator).
        /// <para>Range: 0 - 0xFFFFFFFF.</para>
        /// <para>Default: 0xFFFF(Coordinator), 0 (Router/End Device).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DestinationAddressLow = 0x444C,

        /// <summary>
        /// MY.
        /// <para>Read the 16-bit network address of the module.</para>
        /// </summary>
        /// <remarks>
        /// A value of 0xFFFE means the module has not joined a ZigBee network.
        /// <para>Range: 0 - 0xFFFEƒ [read-only].</para>
        /// <para>Default: 0xFFFE.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NetworkAddress = 0x4D59,

        /// <summary>
        /// MP.
        /// <para>Read the 16-bit network address of the module's parent.</para>
        /// </summary>
        /// <remarks>
        /// A value of 0xFFFE means the module does not have a parent.
        /// <para>Range: 0 - 0xFFFEƒ [read-only].</para>
        /// <para>Default: 0xFFFE.</para>
        /// <para>Applicable: End Device.</para>
        /// </remarks>
        ParentAddress = 0x4D50,

        /// <summary>
        /// NC.
        /// <para>Read the number of end device children that can join the device.</para>
        /// </summary>
        /// <remarks>
        /// If NC returns 0, then the device cannot allow any more end device children to join.
        /// <para>Range: 0 - MAX_CHILDREN (maximum varies).</para>
        /// <para>Default: read-only.</para>
        /// <para>Applicable: Coordinator, Router.</para>
        /// </remarks>
        NumberOfRemainingChildren = 0x4E43,

        /// <summary>
        /// SH.
        /// <para>Read the high 32 bits of the module's unique 64-bit address.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 0xFFFFFFFF ƒ[read-only].</para>
        /// <para>Default: factory-set.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SerialNumberHigh = 0x5348,

        /// <summary>
        /// SL.
        /// <para>Read the low 32 bits of the module's unique 64-bit address.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 - 0xFFFFFFFF ƒ[read-only].</para>
        /// <para>Default: factory-set.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SerialNumberLow = 0x534C,

        /// <summary>
        /// NI.
        /// <para>Set/read a string identifier</para>
        /// </summary>
        /// <remarks>
        /// The register only accepts printable ASCII data. 
        /// In AT Command Mode, a string cannot start with a space. 
        /// A carriage return ends the command. 
        /// A command will automatically end when maximum bytes for the string have been entered. 
        /// This string is returned as part of the ND (Node Discover) command. 
        /// This identifier is also used with the DN (Destination Node) command. 
        /// In AT command mode, an ASCII comma (0x2C) cannot be used in the NI string.
        /// <para>Range: 20-Byte printable ƒASCII string.</para>
        /// <para>Default: ASCII space character (0x20).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NodeIdentifier = 0x4E49,

        /// <summary>
        /// SE.
        /// <para>Set/read the ZigBee application layer source endpoint value.</para>
        /// </summary>
        /// <remarks>
        /// This value will be used as the source endpoint for all data transmissions. 
        /// SE is only supported in AT firmware. 
        /// The default value 0xE8 (Data endpoint) is the Digi data endpoint.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0xE8.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SourceEndpoint = 0x5345,

        /// <summary>
        /// DE.
        /// <para>Set/read Zigbee application layer destination ID value.</para>
        /// </summary>
        /// <remarks>
        /// This value will be used as the destination endpoint all data transmissions. 
        /// DE is only supported in AT firmware. 
        /// The default value (0xE8) is the Digi data endpoint.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0xE8.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DestinationEndpoint = 0x4445,

        /// <summary>
        /// CI.
        /// <para>Set/read Zigbee application layer cluster ID value.</para>
        /// </summary>
        /// <remarks>
        /// This value will be used as the cluster ID for all data transmissions. 
        /// CI is only supported in AT firmware. 
        /// The default value0x11 (Transparent data cluster ID).
        /// <para>Range: 0 - 0xFFFF.</para>
        /// <para>Default: 0x11.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ClusterIdentifier = 0x4349,

        /// <summary>
        /// NP.
        /// <para>This value returns the maximum number of RF payload bytes that can be sent in a unicast transmission.</para>
        /// </summary>
        /// <remarks>
        /// If APS encryption is used (API transmit option bit enabled), the maximum payload size is reduced by 9 bytes. 
        /// If source routing is used (AR less than 0xFF), the maximum payload size is reduced further. 
        /// Note: NP returns a hexadecimal value. (e.g. if NP returns 0x54, this is equivalent to 84 bytes). 
        /// <para>Range: 0 - 0xFFFF.</para>
        /// <para>Default: [read-only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        MaxRFPayloadBytes = 0x4E50,

        /// <summary>
        /// DD.
        /// <para>Stores a device type value</para>
        /// </summary>
        /// <remarks>
        /// This value can be used to differentiate different XBee-based devices. 
        /// Digi reserves the range 0 - 0xFFFFFF.
        /// <para>Range: 0 - 0xFFFFFFFF.</para>
        /// <para>Default: 0x30000.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        /// <returns><see cref="DeviceType"/></returns>
        DeviceTypeIdentifier = 0x4444,

        #endregion

        #region Networking

        /// <summary>
        /// CH.
        /// <para>Set/Read the channel number used for transmitting and receiving data between RF modules</para>
        /// </summary>
        /// <remarks>
        /// Uses 802.15.4 channel numbers. A value of 0 means the device 
        /// has not joined a PAN and is not operating on any channel.
        /// <para>Range: 
        /// XBee 0, 0x0B - 0x1A (Channels 11-26),
        /// XBee-PRO (S2) 0, 0x0B - 0x18 (Channels 11-24), 
        /// XBee-PRO (S2B) 0, 0x0B - 0x19 (Channels 11-25).
        /// </para>
        /// <para>Default: [read-only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        OperatingChannel = 0x4348,

        /// <summary>
        /// DA.
        /// <para>End device will immediately disassociate from a Coordinator (if associated) and reattempt to associate.</para>
        /// </summary>
        /// <remarks>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ForceDisassociation = 0x4441,

        /// <summary>
        /// ID. 
        /// <para>Set/read the 64-bit extended PAN ID.</para>
        /// </summary>
        /// <remarks>
        /// If set to 0, the coordinator will select a random extended PAN ID, 
        /// and the router / end device will join any extended PAN ID. 
        /// Changes to ID should be written to non-volatile memory using the 
        /// WR command to preserve the ID setting if a power cycle occurs.
        /// <para>Range: 0 - 0xFFFFFFFFFFFFFFFF.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ExtendedPanId = 0x4944,

        /// <summary>
        /// OP.
        /// <para>Read the 64-bit extended PAN ID</para>
        /// </summary>
        /// <remarks>
        /// The OP value reflects the operating extended PAN ID that the module is running on. 
        /// If ID > 0, OP will equal ID.
        /// <para>Range: 0x01 - 0xFFFFFFFFFFFFFFFF.</para>
        /// <para>Default: [read-only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        OperatingExtendedPanId = 0x4F50,

        /// <summary>
        /// NH.
        /// <para>Set / read the maximum hops limit.</para>
        /// </summary>
        /// <remarks>
        /// This limit sets the maximum broadcast hops value (BH) and determines the unicast timeout. 
        /// The timeout is computed as (50 * NH) + 100 ms. The default unicast timeout of 1.6 seconds 
        /// (NH=0x1E) is enough time for data and the acknowledgment to traverse about 8 hops.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0x1E.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        MaximumUnicastHops = 0x4E48,

        /// <summary>
        /// BH.
        /// <para>Set/Read the maximum number of hops for each broadcast data
        /// transmission. Setting this to 0 will use the maximum number of hops</para>
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0x1E.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </summary>
        BroadcastHops = 0x4248,

        /// <summary>
        /// OI.
        /// <para>Read the 16-bit PAN ID.</para>
        /// </summary>
        /// <remarks>
        /// The OI value reflects the actual 16-bit PAN ID the module is running on.
        /// <para>Range: 0 - 0xFFFF.</para>
        /// <para>Default: [read-only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        OperatingPanId = 0x4F49,

        /// <summary>
        /// NT.
        /// <para>Set/Read the node discovery timeout</para>
        /// </summary>
        /// <remarks>
        /// When the network discovery (ND) command is issued, the NT value is included 
        /// in the transmission to provide all remote devices with a response timeout. 
        /// Remote devices wait a random time, less than NT, before sending their response.
        /// <para>Range: 0x20 - 0xFF [x 100 msec].</para>
        /// <para>Default: 0x3C (60 dec).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NodeDiscoverTimeout = 0x4E54,

        /// <summary>
        /// NO.
        /// <para>Set/Read the options value for the network discovery command.</para>
        /// </summary>
        /// <remarks>
        /// The options bitfield value can change the behavior of the ND (network 
        /// discovery) command and/or change what optional values are returned in any received 
        /// ND responses or API node identification frames. Options include: 
        /// 0x01 = Append DD value (to ND responses or API node identification frames), 
        /// 002 = Local device sends ND response frame when ND is issued.
        /// <para>Range: 0 - 0x03 [bitfield].</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NetworkDiscoveryOptions = 0x4E4F,

        /// <summary>
        /// SC.
        /// <para>Set/Read bit field list of channels to scan.</para>
        /// </summary>
        /// <remarks>
        /// Changes to SC should be written using WR command to preserve the SC setting if a power cycle occurs.
        /// <para>Coordinator - Bit field list of channels to choose from prior to starting network.</para>
        /// <para>Router/End Device - Bit field list of channels that will be scanned to find a Coordinator/Router to join.</para>
        /// <para>Range: 
        /// XBee 1 - 0xFFFF [bitfield], 
        /// XBee-PRO (S2) 1 - 0x3FFF [bitfield] (bits 14, 15 not allowed), 
        /// XBee-PRO (S2B) 1-0x7FFF (bit 15 is not allowed).
        /// </para>
        /// <para>Default: 1FFE.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ScanChannels = 0x5343,

        /// <summary>
        /// SD.
        /// <para>Set/Read the scan duration exponent.</para>
        /// </summary>
        /// <remarks>
        /// Scan Time is measured as: (# Channels to Scan) * (2 ^ SD) * 15.36ms - The number of 
        /// channels to scan is determined by the SC parameter. The XBee can scan up to 16 channels 
        /// (SC = 0xFFFF). Changes to SD should be written using WR command. 
        /// <para>Coordinator - Duration of the Active and Energy Scans (on each channel) that are 
        /// used to determine an acceptable channel and Pan ID for the Coordinator to startup on.ƒ</para>
        /// <para>Router / End Device - Duration of Active Scan (on each channel) used to locate an 
        /// available Coordinator / Router to join during Association.</para>
        /// <para>Note: SD influences the time the MAC listens for beacons or runs an energy scan on a 
        /// given channel. The SD time is not a good estimate of the router/end device joining time 
        /// requirements. ZigBee joining adds additional overhead including beacon processing on 
        /// each channel, sending a join request, etc. that extend the actual joining time.</para>
        /// <para>Range: 0 - 7 [exponent].</para>
        /// <para>Default: 3.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ScanDuration = 0x5344,

        /// <summary>
        /// ZS.
        /// <para>Set / read the ZigBee stack profile value.</para>
        /// </summary>
        /// <remarks>
        /// This must be set the same on all devices that should join the same network.
        /// <para>Range: 0 - 2.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ZigBeeStackProfile = 0x5A53,

        /// <summary>
        /// NJ.
        /// <para>Set/Read the time that a Coordinator/Router allows nodes to join.</para>
        /// </summary>
        /// <remarks>
        /// This value can be changed at run time without requiring a Coordinator or Router to
        /// restart. The time starts once the Coordinator or Router has started. The timer is reset
        /// on power-cycle or when NJ changes.
        /// <para>For an end device to enable rejoining, NJ should be set less than 0xFF on the device 
        /// that will join. If NJ is less then 0xFF, the device assumes the network is not allowing joining and 
        /// first tries to join a network using rejoining. If multiple rejoining attempts fail, or if 
        /// NJ=0xFF, the device will attempt to join using association.</para>
        /// <para>Note: Setting the NJ command will not cause the radio to broadcast the new value of 
        /// NJ out to the network via a Mgmt_Permit_Joining_req; this value is transmitted by setting CB=2. 
        /// See the command description for CB for more information.</para>
        /// <para>Range: 0 - 0xFF ƒ[x 1 sec].</para>
        /// <para>Default: 0xFF ƒ(always allows joining).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NodeJoinTime = 0x4E4A,

        /// <summary>
        /// JV.
        /// <para>Set/Read the channel verification parameter.</para>
        /// </summary>
        /// <remarks>
        /// If JV=1, a router will verify the coordinator is on its operating channel when joining or coming up from a
        /// power cycle. If a coordinator is not detected, the router will leave its current channel and
        /// attempt to join a new PAN. If JV=0, the router will continue operating on its current
        /// channel even if a coordinator is not detected.
        /// <para>Range: 0 (disabled), 1 (enabled).</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Router.</para>
        /// </remarks>
        ChannelVerification = 0x4A56,

        /// <summary>
        /// NW.
        /// <para>Set/read the network watchdog timeout value.</para>
        /// </summary>
        /// <remarks>
        /// If NW is set greater than 0, the router will monitor communication from the coordinator (or data collector) 
        /// and leave the network if it cannot communicate with the coordinator for 3 NW periods. 
        /// The timer is reset each time data is received from or sent to a coordinator, or if a manyto-one broadcast is received.
        /// <para>Range: 0 - 0x64FF [x 1 minute] (up to over 17 days).</para>
        /// <para>Default: 0 (disabled).</para>
        /// <para>Applicable: Router.</para>
        /// </remarks>
        NetworkWatchdogTimeout = 0x4E57,

        /// <summary>
        /// JN.
        /// <para>Set / read the join notification setting.</para>
        /// </summary>
        /// <remarks>
        /// If enabled, the module will transmit a broadcast node identification packet on power up and when joining. 
        /// This action blinks the Associate LED rapidly on all devices that receive the transmission, and
        /// sends an API frame out the UART of API devices. This feature should be disabled for large networks to 
        /// prevent excessive broadcasts.
        /// <para>Range: 0 - 1.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Router, End Device.</para>
        /// </remarks>
        JoinNotification = 0x4A4E,

        /// <summary>
        /// AR.
        /// <para>Set/read time between consecutive aggregate route broadcast messages.</para>
        /// </summary>
        /// <remarks>
        /// If used, AR may be set on only one device to enable many-to-one routing to the device. 
        /// Setting AR to 0 only sends one broadcast. AR is in units of 10 seconds.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0xFF.</para>
        /// <para>Applicable: Coordinator, Router.</para>
        /// </remarks>
        AggregateRoutingNotification = 0x4152,

        /// <summary>
        /// DJ.
        /// <para>Setting this register to 1 will disable the device from joining.</para>
        /// </summary>
        /// <remarks>
        /// This setting is not writeable (WR) and will reset to zero after a power cycle.
        /// <para>Range: 1 (enabled).</para>
        /// <para>Default: 0 [not writeable].</para>
        /// <para>Applicable: Router, End Device.</para>
        /// </remarks>
        DisableJoining = 0x444A,

        /// <summary>
        /// II.
        /// <para>This register determines the operating 16-bit PAN ID for the network.</para>
        /// </summary>
        /// <remarks>
        /// Changing this value will cause the Coordinator to leave the network and form another. 
        /// A setting of 0xFFFF allows the system to choose its own 16 bit 802.15.4 PAN Identifier. 
        /// This setting is not writeable (WR) and will reset to 0xFFFF after a power cycle.
        /// <para>Range: 0x0000 - 0xFFFF.</para>
        /// <para>Default: 0xFFFF [not writeable].</para>
        /// <para>Applicable: Coordinator.</para>
        /// </remarks>
        InitialId = 0x4949,

        #endregion

        #region Security

        /// <summary>
        /// EE.
        /// <para>Set/Read the encryption enable setting.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 0 (disabled), 1 (enabled).</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        EncryptionEnable = 0x4545,

        /// <summary>
        /// EO.
        /// <para>Options for encryption.</para>
        /// </summary>
        /// <remarks>
        /// Unused option bits should be set to 0. Options include: 
        /// 0x01 - Send the security key unsecured over-the-air during joins, 
        /// 0x02 - Use trust center (coordinator only).
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        EncryptionOptions = 0x454F,

        /// <summary>
        /// NK.
        /// <para>Set the 128-bit AES network encryption key.</para>
        /// </summary>
        /// <remarks>
        /// This command is write-only; NK cannot be read. If set to 0 (default), 
        /// the module will select a random network key. 
        /// <para>Range: 128-bit value.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator.</para>
        /// </remarks>
        EncryptionKey = 0x4E4B,

        /// <summary>
        /// KY.
        /// <para>Set the 128-bit AES link key.</para>
        /// </summary>
        /// <remarks>
        /// This command is write only; KY cannot be read. Setting KY to 0 will cause the 
        /// coordinator to transmit the network key in the clear to joining devices, 
        /// and will cause joining devices to acquire the network key in the clear when joining.
        /// <para>Range: 128-bit value.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        LinkKey = 0x4B59,

        #endregion

        #region RF Interfacing

        /// <summary>
        /// PL.
        /// <para>Select/Read the power level at which the RF module transmits conducted power.</para>
        /// </summary>
        /// <remarks>
        /// For XBee-PRO (S2B) Power Level 4 is calibrated and the other power levels are approximate. 
        /// For XBee (S2), onlythe default power level (PL=4) is guaranteed from -40 to 85° C.
        /// <para>Range: 0-4.</para>
        /// <para>Default: 4.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        PowerLevel = 0x504C,

        /// <summary>
        /// PM.
        /// <para>Set/read the power mode of the device.</para>
        /// </summary>
        /// <remarks>
        /// Enabling boost mode will improve the receive sensitivity by 1dB 
        /// and increase the transmit power by 2dB. 
        /// <para>Note: Enabling boost mode on the XBee-PRO will not affect the output power. Boost
        /// mode imposes a slight increase in current draw. See section 1.2 for details.</para>
        /// <para>Range: 0 (boos mode disabled), 1 (boost mode enabled).</para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        PowerMode = 0x504D,

        /// <summary>
        /// DB.
        /// <para>This command reports the received signal strength of the last received RF data packet.</para>
        /// </summary>
        /// <remarks>
        /// The DB command only indicates the signal strength of the last hop. 
        /// It does not provide an accurate quality measurement for a multihop link. 
        /// DB can be set to 0 to clear it. The DB command value is measured in -dBm. 
        /// As of 2x6x firmware, the DB command value is also updated when an APS acknowledgment is received.
        /// Observed values for XBee-PRO: 0x1A - 0x58, XBee: 0x 1A - 0x5C.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        /// <example>
        /// If DB returns 0x50, then the RSSI of the last packet received was -80dBm.
        /// </example>
        ReceivedSignalStrength = 0x4442,

        /// <summary>
        /// PP.
        /// <para>Read the dBm output when maximum power is selected (PL4).</para>
        /// </summary>
        /// <remarks>
        /// Enabling boost mode will improve the receive sensitivity by 1dB 
        /// and increase the transmit power by 2dB. 
        /// <para>Note: Enabling boost mode on the XBee-PRO will not affect the output power. Boost
        /// mode imposes a slight increase in current draw. See section 1.2 for details.</para>
        /// <para>Range: 0x00 - 0x12.</para>
        /// <para>Default: [read only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        PeakPower = 0x5050,

        #endregion

        #region Serial Interfacing

        /// <summary>
        /// AP.
        /// <para>Enable API Mode.</para>
        /// </summary>
        /// <remarks>
        /// The AP command is only supported when using API firmware: 21xx (API coordinator), 23xx (API router), 29xx (API end device).
        /// <para>Range: 1 (enabled), 2 (enabled w/escaped control characters).</para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ApiEnable = 0x4150,

        /// <summary>
        /// AO.
        /// <para>Configure options for API.</para>
        /// </summary>
        /// <remarks>
        /// Current options select the type of receive API frame to send out the Uart for received RF data packets.
        /// <para>Range: 
        /// 0 (default receive API indicators enabled), 
        /// 1 (explicit Rx data indicator API frame enabled (0x91)), 
        /// 3 (enable ZDO passthrough of ZDO requests to the UART which are not supported by the stack, 
        /// as well as Simple_Desc_req, Active_EP_req, and Match_Desc_req).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ApiOptions = 0x414F,

        /// <summary>
        /// BD.
        /// <para>Set/Read the serial interface data rate for communication between the module serial port and host.</para>
        /// </summary>
        /// <remarks>
        /// Any value above 0x07 will be interpreted as an actual baud rate. When a value above 0x07 is sent, the closest
        /// interface data rate represented by the number is stored in the BD register.
        /// <para>Range: 
        /// 0 - 7 (standard baud rates), 
        /// 0x80 - 0xE1000 (non-standard baud rates up to 921 Kbps).
        /// </para>
        /// <para>Default: 3 (9600 Kbps).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        InterfaceDataRate = 0x4244,

        /// <summary>
        /// NB.
        /// <para>Set/Read the serial parity setting on the module.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 = No parity 
        /// 1 = Even parity 
        /// 2 = Odd parity 
        /// 3 = Mark parity.
        /// </para>
        /// <para>Default: 0 (No parity).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SerialParity = 0x4E42,

        /// <summary>
        /// SB.
        /// <para>Set/read the number of stop bits for the UART.</para>
        /// </summary>
        /// <remarks>
        /// Two stop bits are not supported if mark parity is enabled.
        /// <para>Range: 0 (1 stop bit), 1 (2 stop bits).</para>
        /// <para>Default: 0 (1 stop bit).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        StopBits = 0x5342,

        /// <summary>
        /// RO.
        /// <para>Set/Read number of character times of inter-character silence required before packetization.</para>
        /// </summary>
        /// <remarks>
        /// Set (RO=0) to transmit characters as they arrive instead of buffering them into one RF packet The RO 
        /// command is only supported when using AT firmware: 20xx (AT coordinator), 22xx (AT router), 28xx (AT end device).
        /// <para>Range: 0 - 0xFF [x character times].</para>
        /// <para>Default: 3.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        PacketizationTimeout = 0x524F,

        /// <summary>
        /// D7.
        /// <para>Select/Read options for the DIO7 line of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// Options include CTS flow control and I/O line settings.
        /// Introduced in firmware v1.x80.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (CTS Flow Controlƒ), 
        /// 3 (Digital input), 
        /// 4 (Digital output low), 
        /// 5 (Digital output high), 
        /// 6 (RS-485 transmit enable - low enable), 
        /// 7 (RS-485 tranmist enable - high enable).
        /// </para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO7Config = 0x4437,

        /// <summary>
        /// D6.
        /// <para>Configure options for the DIO6 line of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// Options include RTS flow control and I/O line settings.
        /// Introduced in firmware v1.x80.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (RTS Flow Controlƒ), 
        /// 3 (Digital input), 
        /// 4 (Digital output low), 
        /// 5 (Digital output high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO6Config = 0x4436,

        #endregion

        #region I/O Commands

        /// <summary>
        /// IR.
        /// <para>Set/Read the IO sample rate to enable periodic sampling.</para>
        /// </summary>
        /// <remarks>
        /// For periodic sampling to be enabled, IR must be set to a non-zero value, 
        /// and at least one module pin must have analog or digital IO functionality 
        /// enabled (see D0-D8, P0-P2 commands). The sample rate is measured in milliseconds.
        /// <para>Range: 0, 0x32 - 0xFFFF (ms).</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SampleRate = 0x4952,

        /// <summary>
        /// IC.
        /// <para>Set/Read the digital IO pins to monitor for changes in the IO state.</para>
        /// </summary>
        /// <remarks>
        /// IC works with the individual pin configuration commands (D0-D8, P0-P2). 
        /// If a pin is enabled as a digital input/output, the IC command can be used to force 
        /// an immediate IO sample transmission when the DIO state changes. IC is a bitmask 
        /// that canbe used to enable or disable edge detection on individual channels. 
        /// Unused bits should be set to 0.
        /// <para>Range: 0 - 0xFFFF.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIOChangeDetect = 0x4943,

        /// <summary>
        /// P0.
        /// <para>Select/Read function for PWM0.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (RSSI PWM), 
        /// 3 (Digital input, monitored), 
        /// 4 (Digital output, default low), 
        /// 5 (Digital output, default high).
        /// </para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        PWM0Config = 0x5130,

        /// <summary>
        /// P1.
        /// <para>Configure options for the DIO11 line of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Unmonitored digital input),  
        /// 3 (Digital input, monitored), 
        /// 4 (Digital output, default low), 
        /// 5 (Digital output, default high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO11Config = 0x5131,

        /// <summary>
        /// P2.
        /// <para>Configure options for the DIO12 line of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Unmonitored digital input),  
        /// 3 (Digital input, monitored), 
        /// 4 (Digital output, default low), 
        /// 5 (Digital output, default high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO12Config = 0x5132,

        /// <summary>
        /// P3.
        /// <para>Set/Read function for DIO13.</para>
        /// </summary>
        /// <remarks>
        /// This command is not yet supported.
        /// <para>Range: 
        /// 0 (Disabled),  
        /// 3 (Digital input), 
        /// 4 (Digital output, low), 
        /// 5 (Digital output, high).
        /// </para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO13Config = 0x5133,

        /// <summary>
        /// D0.
        /// <para>Select/Read function for AD0/DIO0.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 1 (Commissioning button enabled), 
        /// 2 (Analog input, single ended), 
        /// 3 (Digital input), 
        /// 4 (Digital output, low), 
        /// 5 (Digital output, high).
        /// </para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO0Config = 0x4430,

        /// <summary>
        /// D1.
        /// <para>Select/Read function for AD1/DIO1.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (Analog input, single ended), 
        /// 3 (Digital input), 
        /// 4 (Digital output, low), 
        /// 5 (Digital output, high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO1Config = 0x4431,

        /// <summary>
        /// D2.
        /// <para>Select/Read function for AD2/DIO2.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (Analog input, single ended), 
        /// 3 (Digital input), 
        /// 4 (Digital output, low), 
        /// 5 (Digital output, high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO2Config = 0x4432,

        /// <summary>
        /// D3.
        /// <para>Select/Read function for AD3/DIO3.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 2 (Analog input, single ended), 
        /// 3 (Digital input), 
        /// 4 (Digital output, low), 
        /// 5 (Digital output, high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO3Config = 0x4433,

        /// <summary>
        /// D4.
        /// <para>Select/Read function for DIO4.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 3 (Digital input), 
        /// 4 (Digital output, low), 
        /// 5 (Digital output, high).
        /// </para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO4Config = 0x4434,

        /// <summary>
        /// D5.
        /// <para>Configure options for the DIO5 line of the RF module.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 1 (Associated indication LED), 
        /// 3 (Digital input), 
        /// 4 (Digital output, default low), 
        /// 5 (Digital output, default high).
        /// </para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO5Config = 0x4435,

        /// <summary>
        /// D8.
        /// <para>Set/Read function for DIO8.</para>
        /// </summary>
        /// <remarks>
        /// This command is not yet supported.
        /// <para>Range: 
        /// 0 (Disabled), 
        /// 3 (Digital input), 
        /// 4 (Digital output, default low), 
        /// 5 (Digital output, default high).
        /// </para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DIO8Config = 0x4438,

        /// <summary>
        /// LT.
        /// <para>Set/Read the Associate LED blink time.</para>
        /// </summary>
        /// <remarks>
        /// If the Associate LED functionality is enabled (D5 command), this value determines 
        /// the on and off blink times for the LED when the module has joined a network. 
        /// If LT=0, the default blink rate will be used (500ms coordinator, 250ms router/end device). 
        /// For all other LT values, LT is measured in 10ms.
        /// <para>Range: 0, 0x0A - 0xFF (100 - 2550 ms).</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        AssociateLedBlinkTime = 0x4C54,

        /// <summary>
        /// PR.
        /// <para>Set/Read bitfield to configure internal pull-up resistor status for I/O lines</para>
        /// </summary>
        /// <remarks>
        /// Bit set to '1' specifies pull-up enabled, '0' specifies no pull-up. 30k pull-up resistors.
        /// <para>Range: 0 - 0x3FFF.</para>
        /// <para>Default: 0 - 0x1FFF.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        PullUpResistor = 0x4C52,

        /// <summary>
        /// RP.
        /// <para>Time the RSSI signal will be output on the PWM after the last RF data 
        /// reception or APS acknowledgment.</para>
        /// </summary>
        /// <remarks>
        /// When RP = 0xFF, output will always be on.
        /// <para>Range: 0 - 0xFF [x 100 ms].</para>
        /// <para>Default: 0x28 (40 dec).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        RssiPwmTimer = 0x524C,

        /// <summary>
        /// %V.
        /// <para>Reads the voltage on the Vcc pin.</para>
        /// <remarks>
        /// Scale by 1200/1024 to convert to mV units. 
        /// <para>Range: 0x00 - 0xFFFF [read only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        /// <example>
        /// A %V reading of 0x900 (2304 decimal) represents 2700mV or 2.70V.
        /// </example>
        /// </summary>
        SupplyVoltage = 0x2556,

        /// <summary>
        /// V+.
        /// <para>The voltage supply threshold is set with the V+ command.</para>
        /// <remarks>
        /// If the measured supply voltage falls below or equal to this threshold, the supply voltage 
        /// will be included in the IO sample set. V+ is set to 0 by default (do not include the 
        /// supply voltage). Scale mV units by 1024/1200 to convert to internal units. 
        /// Given the operating Vcc ranges for different platforms, and scaling by 1024/1200, 
        /// the useful parameter ranges are: 
        /// XBee 2100-3600 mV 0,0x0700-0x0c00, 
        /// PRO 3000-3400 mV, 0,0x0a00-0x0b55, 
        /// S2B 2700-3600 mV, 0,0x0900-0x0c00.
        /// <para>Range: 0-0xFFFF.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        /// <example>
        /// For example, for a 2700mV threshold enter 0x900.
        /// </example>
        /// </summary>
        VoltageSupplyMonitoring = 0x562B,

        /// <summary>
        /// TP.
        /// <para>Reads the module temperature in Degrees Celsius.</para>
        /// <remarks>
        /// Accuracy +/- 7 degrees. 
        /// 1° C = 0x0001 and -1° C = 0xFFFF. 
        /// Command is only available in PRO S2B.
        /// <para>Range: 0x0 - 0xFFFF.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        /// </summary>
        Temperature = 0x5451,

        #endregion

        #region Diagnostics

        /// <summary>
        /// VR.
        /// <para>Read firmware version of the module.</para>
        /// </summary>
        /// <remarks>
        /// The firmware version returns 4 hexadecimal values (2 bytes) "ABCD". 
        /// Digits ABC are the main release number and D is the revision number from the main release. 
        /// "B" is a variant designator. 
        /// XBee and XBee-PRO ZB modules return: 0x2xxx versions. 
        /// XBee and XBee-PRO ZNet modules return: 0x1xxx versions. 
        /// ZNet firmware is not compatible with ZB firmware.
        /// <para>Range: 0 - 0xFFFF [read-only].</para>
        /// <para>Default: Factory-set.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        FirmwareVersion = 0x5652,

        /// <summary>
        /// HV.
        /// <para>Read the hardware version of the module.version of the module.</para>
        /// </summary>
        /// <remarks>
        /// This command can be used to distinguish among different hardware platforms. 
        /// The upper byte returns a value that is unique to each module type. 
        /// The lower byte indicates the hardware revision. XBee ZB and XBee ZNet 
        /// modules return the following (hexadecimal) values:
        /// 0x19xx - XBee module, 
        /// 0x1Axx - XBee-PRO module.
        /// <para>Range: 0 - 0xFFFF [read-only].</para>
        /// <para>Default: Factory-set.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        HardwareVersion = 0x4856,

        /// <summary>
        /// AI.
        /// <para>Read information regarding last node join request.</para>
        /// </summary>
        /// <remarks>
        /// New non-zero AI values may be added in later firmware versions. 
        /// Applications should read AI until it returns 0x00, indicating 
        /// a successful startup (coordinator) or join (routers and end devices)
        /// <para>Range: 0 - 0xFF [read-only].</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        /// <returns><see cref="AssociationStatus"/></returns>
        AssociationIndication = 0x4149,

        #endregion

        #region AT Command Options

        /// <summary>
        /// CT.
        /// <para>Set/Read the period of inactivity (no valid commands received) after which 
        /// the RF module automatically exits AT Command Mode and returns to Idle Mode.</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 2 - 0x028F [x 100 ms].</para>
        /// <para>Default: 0x64 (100 dec).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        CommandModeTimeout = 0x4354,

        /// <summary>
        /// CN.
        /// <para>Explicitly exit the module from AT Command Mode.</para>
        /// </summary>
        /// <remarks>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ExitCommandMode = 0x434E,

        /// <summary>
        /// GT.
        /// <para>Set required period of silence before and after the Command Sequence
        /// Characters of the AT Command Mode Sequence (GT+ CC + GT). The period of silence
        /// is used to prevent inadvertent entrance into AT Command Mode</para>
        /// </summary>
        /// <remarks>
        /// <para>Range: 2 - 0x0CE4 [x 1 ms] (max of 3.3 decimal sec).</para>
        /// <para>Default: 0x3E8 (1000 dec).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        GuardTimes = 0x4754,

        /// <summary>
        /// CC.
        /// <para>Set/Read the ASCII character value to be used between Guard Times of the 
        /// AT Command Mode Sequence (GT+CC+GT).</para>
        /// </summary>
        /// <remarks>
        /// The AT Command Mode Sequence enters the RF module into AT Command Mode. 
        /// The CC command is only supported when using AT firmware: 20xx (AT coordinator), 
        /// 22xx (AT router), 28xx (AT end device).
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0x2B ('+' in ASCII).</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        CommandSequenceCharacter = 0x4343,

        #endregion

        #region Sleep Commands

        /// <summary>
        /// SM.
        /// <para>Sets the sleep mode on the RF module.</para>
        /// </summary>
        /// <remarks>
        /// An XBee loaded with router firmware can be configured as either a router (SM set to 0) 
        /// or an end device (SM > 0). Changing a device from a router to an end device (or vice versa) 
        /// forces the device to leave the network and attempt to join as the new device type when 
        /// changes are applied.
        /// <para>Range: 
        /// 0 - Sleep disabled (router), 
        /// 1 - Pin sleep enabled, 
        /// 4 - Cyclic sleep enabled, 
        /// 5 - Cyclic sleep, pin wake.
        /// </para>
        /// <para>Default: 0 (Router), 4 (End Device).</para>
        /// <para>Applicable: Router, End Device.</para>
        /// </remarks>
        SleepMode = 0x534D,

        /// <summary>
        /// SN.
        /// <para>Sets the number of sleep periods to not assert the On/Sleep
        /// pin on wakeup if no RF data is waiting for the end device.</para>
        /// </summary>
        /// <remarks>
        /// This command allows a host application to sleep for an extended time if no RF data is present.
        /// <para>Range: 1 - 0xFFFF.</para>
        /// <para>Default: 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NumberOfSleepPeriods = 0x534E,

        /// <summary>
        /// SP.
        /// <para>This value determines how long the end device will sleep at a time, up to
        /// 28 seconds.</para>
        /// </summary>
        /// <remarks>
        /// The sleep time can effectively be extended past 28 seconds using the SN
        /// command. On the parent, this value determines how long the parent will buffer a
        /// message for the sleeping end device. It should be set at least equal to the longest SP
        /// time of any child end device.
        /// <para>Range: 0x20 - 0xAF0 x 10ms (Quarter second resolution).</para>
        /// <para>Default: 0x20.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SleepPeriod = 0x534C,

        /// <summary>
        /// ST.
        /// <para>Sets the time before sleep timer on an end device.</para>
        /// </summary>
        /// <remarks>
        /// The timer is reset each time serial or RF data is received. Once the timer expires, 
        /// an end device may enter low power operation. Applicable for cyclic sleep end devices only.
        /// <para>Range: 1 - 0xFFFE (x 1ms).</para>
        /// <para>Default: 0x1388 (5 seconds).</para>
        /// <para>Applicable: End Device.</para>
        /// </remarks>
        TimeBeforeSleep = 0x5354,

        /// <summary>
        /// SO.
        /// <para>Configure options for sleep.</para>
        /// </summary>
        /// <remarks>
        /// Unused option bits should be set to 0. Sleep options include: 
        /// 0x02 - Always wake for ST time, 
        /// 0x04 - Sleep entire SN * SP time, 
        /// 0x06 - Enabled extended sleep and wake for ST time.
        /// Sleep options should not be used for most applications. 
        /// See chapter 6 for more information.
        /// <para>Range: 0 - 0xFF.</para>
        /// <para>Default: 0.</para>
        /// <para>Applicable: End Device.</para>
        /// </remarks>
        SleepOptions = 0x534F,

        /// <summary>
        /// WH.
        /// <para>Set/Read the wake host timer value.</para>
        /// </summary>
        /// <remarks>
        /// If the wake host timer is set to a nonzero value, this timer specifies a time 
        /// (in millisecond units) that the device should allow after waking from sleep before 
        /// sending data out the UART or transmitting an IO sample. If serial characters are received, 
        /// the WH timer is stopped immediately.
        /// <para>Range: 0 - 0xFFFF (x 1ms).</para>
        /// <para>Applicable: End Device.</para>
        /// </remarks>
        WakeHost = 0x5748,

        /// <summary>
        /// SI.
        /// <para>Cause a cyclic sleep module to sleep immediately rather than wait
        /// for the ST timer to expire.</para>
        /// </summary>
        /// <remarks>
        /// <para>Applicable: End Device.</para>
        /// </remarks>
        SleepImmediately = 0x5349,

        /// <summary>
        /// PO.
        /// <para>Set/Read the end device poll rate.</para>
        /// </summary>
        /// <remarks>
        /// Setting this to 0 (default) enables polling at 100 ms (default rate). 
        /// Adaptive polling may allow the end device to poll more rapidly for 
        /// a short time when receiving RF data.
        /// <para>Range: 0 - 0x3E8.</para>
        /// <para>Default: 0x00 (100 msec).</para>
        /// <para>Applicable: End Device.</para>
        /// </remarks>
        PoolingRate = 0x504F,

        #endregion

        #region Execution Commands

        /// <summary>
        /// AC.
        /// <para>Applies changes to all command registers causing queued command 
        /// register values to be applied.</para>
        /// </summary>
        /// <remarks>
        /// For example, changing the serial interface rate with the BD command will 
        /// not change the UART interface rate until changes are applied with the AC command. 
        /// The CN command and 0x08 API command frame also apply changes.
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ApplyChanges = 0x4143,

        /// <summary>
        /// WR. 
        /// <para>Write parameter values to non-volatile memory so that parameter modifications 
        /// persist through subsequent resets.</para>
        /// </summary>
        /// <remarks>
        /// Note: Once WR is issued, no additional characters should be sent to the module until 
        /// after the "OK\r" response is received. The WR command should be used sparingly. 
        /// The EM250 supports a limited number of write cycles.
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        Write = 0x5752,

        /// <summary>
        /// RE. 
        /// <para>Restore module parameters to factory defaults.</para>
        /// </summary>
        /// <remarks>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        RestoreDefaults = 0x5245,

        /// <summary>
        /// FR. 
        /// <para>Reset module.</para>
        /// </summary>
        /// <remarks>
        /// Responds immediately with an OK status, and then performs a software 
        /// reset about 2 seconds later.
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        SoftwareReset = 0x4652,

        /// <summary>
        /// NR.
        /// <para>Reset network layer parameters on one or more modules within a PAN.</para>
        /// </summary>
        /// <remarks>
        /// Responds immediately with an 'OK' then causes a network restart. All network
        /// configuration and routing information is consequently lost.
        /// If NR = 0: Resets network layer parameters on the node issuing the command.
        /// If NR = 1: Sends broadcast transmission to reset network 
        /// layer parameters on all nodes in the PAN.
        /// <para>Range: 0 - 1.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NetworkReset = 0x4E52,

        //*************************************************
        // SI command can be found in Sleep Commands region
        //*************************************************

        /// <summary>
        /// CB.
        /// <para>This command can be used to simulate commissioning button presses in software.</para>
        /// </summary>
        /// <remarks>
        /// The parameter value should be set to the number of button presses to be simulated. 
        /// For example, sending the ATCB1 command will execute the action associated with 
        /// 1 commissioning button press.
        /// <para>
        /// Note: Setting CB=2 will send out the new value of NJ unless NJ has been set to either 0 
        /// or 255 (joining permanently disabled or permanently enabled respectively). 
        /// In these two cases, setting CB=2 enables joining for one minute. 
        /// Joining will be disabled after the minute expires.
        /// </para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        CommissioningPushButton = 0x4342,

        /// <summary>
        /// ND.
        /// <para>Discovers and reports all RF modules found.</para>
        /// </summary>
        /// <remarks>
        /// After (NT * 100) milliseconds, the command ends by returning a carrage return. 
        /// ND also accepts a Node Identifier (NI) as a parameter (optional). In this case, 
        /// only a module that matches the supplied identifier will respond. If ND is sent 
        /// through the API, each response is returned as a separate AT_CMD_Response packet. 
        /// The data consists of the above listed bytes without the carriage return delimiters. 
        /// The NI string will end in a "0x00" null character. The radius of the ND command 
        /// is set by the BH command.
        /// <para>Range: optional 20-Byte ƒNI or MY value.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        NodeDiscover = 0x4E44,

        /// <summary>
        /// DN.
        /// <para>Resolves an NI (Node Identifier) string to a physical address (casesensitive).</para>
        /// </summary>
        /// <remarks>
        /// If there is no response from a module within (NT * 100) milliseconds or a parameter is 
        /// not specified (left blank), the command is terminated and an “ERROR” message is returned. 
        /// In the case of an ERROR, Command Mode is not exited. The radius of the DN command is set 
        /// by the BH command.
        /// <para>Range: up to 20-Byte printable ASCII string.</para>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        DestinationNode = 0x444E,

        /// <summary>
        /// IS.
        /// <para>Forces a read of all enabled digital and analog input lines.</para>
        /// </summary>
        /// <remarks>
        /// <para>Applicable: Coordinator, Router, End Device.</para>
        /// </remarks>
        ForceSample = 0x4953,

        /// <summary>
        /// 1S.
        /// <para>Forces a sample to be taken on an XBee Sensor device.</para>
        /// </summary>
        /// <remarks>
        /// This command can only be issued to an XBee sensor device using an API remote command.
        /// <para>Applicable: Router, End Device.</para>
        /// </remarks>
        XbeeSensorSample = 0x3153,

        #endregion
    }
}