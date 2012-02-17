using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Holds basic information about XBee module and allows to set it's properties using AT commands.
    /// </summary>
    public class XBeeConfiguration
    {
        private readonly XBee _xbee;
        private HardwareVersions _hardwareVersion;
        private string _firmware;
        private ApiModes _apiMode;
        private XBeeAddress64 _serialNumber;

        public HardwareVersions HardwareVersion
        {
            get { return _hardwareVersion; }
        }

        public string Firmware
        {
            get { return _firmware; }
        }

        public XBeeAddress64 SerialNumber
        {
            get { return _serialNumber; }
        }

        public ApiModes ApiMode
        {
            get { return _apiMode; }
            set { WriteApiMode(value); }
        }

        private XBeeConfiguration(XBee xbee)
        {
            _xbee = xbee;
        }

        /// <summary>
        /// Reads module basic information
        /// </summary>
        /// <param name="xbee">XBee module to read data from</param>
        /// <returns>XBee basic information</returns>
        public static XBeeConfiguration Read(XBee xbee)
        {
            var config = new XBeeConfiguration(xbee);
            config.ReadApiMode();
            config.ReadHardware();
            config.ReadFirmware();
            config.ReadSerialNumber();
            return config;
        }

        /// <summary>
        /// Saves changes permanently in module flash memory.
        /// </summary>
        public void Save()
        {
            SaveSettings.Write(_xbee);
        }

        private void ReadApiMode()
        {
            _apiMode = At.ApiMode.Read(_xbee);
            Logger.Info("ApiMode: " + _apiMode);
        }

        private void WriteApiMode(ApiModes apiMode)
        {
            At.ApiMode.Write(_xbee, apiMode);
            _apiMode = apiMode;
        }

        private void ReadHardware()
        {
            _hardwareVersion = At.HardwareVersion.Read(_xbee);
            Logger.Info("HardwareVersion: " + At.HardwareVersion.GetName(_hardwareVersion));
        }

        private void ReadFirmware()
        {
            _firmware = At.Firmware.Read(_xbee);
            Logger.Info("Firmware: " + _firmware);
        }

        private void ReadSerialNumber()
        {
            _serialNumber = At.SerialNumber.Read(_xbee);
            Logger.Info("SerialNumber: " + _serialNumber);
        }
    }
}