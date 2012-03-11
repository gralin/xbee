namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    public enum DeviceType
    {
        Unknown = 0x30000,
        ConnectPortX8Gateway = 0x30001,
        ConnectPortX4Gateway = 0x30002,
        ConnectPortX2Gateway = 0x30003,
        RS232Adapter = 0x30005,
        RS485Adapter = 0x30006,
        XBeeSensorAdapter = 0x30007,
        WallRouter = 0x30008,
        DigitalIOAdapter = 0x3000A,
        AnalogIOAdapter = 0x3000B,
        XStick = 0x3000C,
        SmartPlug = 0x3000F,
        XBeeLargeDisplay = 0x30011,
        XBeeSmallDisplay = 0x30012
    }
}