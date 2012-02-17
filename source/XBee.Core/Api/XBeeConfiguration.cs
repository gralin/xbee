using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Holds basic information about XBee module and allows to set it's properties using AT commands.
    /// </summary>
    public class XBeeConfiguration
    {
        private readonly XBee _xbee;
        private HardwareVersion.RadioType _radioType;
        private string _firmware;
        private ApiMode _apiMode;
        private XBeeAddress64 _serialNumber;

        public HardwareVersion.RadioType RadioType
        {
            get { return _radioType; }
        }

        public string Firmware
        {
            get { return _firmware; }
        }

        public XBeeAddress64 SerialNumber
        {
            get { return _serialNumber; }
        }

        public ApiMode ApiMode
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
            config.ReadRadioType();
            config.ReadFirmware();
            config.ReadSerialNumber();
            return config;
        }

        /// <summary>
        /// Saves changes permanently in module flash memory.
        /// </summary>
        public void Save()
        {
            var response = _xbee.Send(new AtCommand(AtCmd.WR));

            if (!response.IsOk)
                throw new XBeeException("Failed to save config");
        }

        private void ReadApiMode()
        {
            var response = _xbee.Send(new AtCommand(AtCmd.AP));

            if (!response.IsOk)
                throw new XBeeException("Failed to read api mode");

            _apiMode = (ApiMode) response.Value[0];

            Logger.Info("ApiMode: " + _apiMode);
        }

        private void WriteApiMode(ApiMode apiMode)
        {
            var response = _xbee.Send(new AtCommand(AtCmd.AP, (int)apiMode));

            if (!response.IsOk)
                throw new XBeeException("Failed to write api mode");

            _apiMode = apiMode;
        }

        private void ReadRadioType()
        {
            var response = _xbee.Send(new AtCommand(AtCmd.HV));

            if (!response.IsOk)
                throw new XBeeException("Failed to read radio type");

            _radioType = HardwareVersion.Parse(response);

            Logger.Info("RadioType: " + HardwareVersion.GetName(_radioType));
        }

        private void ReadFirmware()
        {
            var response = _xbee.Send(new AtCommand(AtCmd.VR));

            if (!response.IsOk)
                throw new XBeeException("Failed to read firmware");

            _firmware = ByteUtils.ToBase16(response.Value);

            Logger.Info("Firmware: " + _firmware);
        }

        private void ReadSerialNumber()
        {
            var data = new OutputStream();
            var sh = _xbee.Send(new AtCommand(AtCmd.SH));
            var sl = _xbee.Send(new AtCommand(AtCmd.SL));
            
            if (!sh.IsOk || !sl.IsOk)
                throw new XBeeException("Failed to read serial number");

            data.Write(sh.Value);
            data.Write(sl.Value);
            _serialNumber = new XBeeAddress64(data.ToArray());

            Logger.Info("SerialNumber: " + _serialNumber);
        }
    }
}