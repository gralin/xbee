﻿using Gadgeteer.Modules.GHIElectronics.Api.At;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Holds basic information about XBee module and allows to set it's properties using AT commands.
    /// </summary>
    public class XBeeConfiguration
    {
        private readonly XBee _xbee;
        private readonly XBeeAddress16 _remoteXbee;

        public HardwareVersions HardwareVersion { get; private set; }
        public string Firmware { get; private set; }
        public XBeeAddress64 SerialNumber { get; private set; }
        public ApiModes ApiMode { get; private set; }
        public string NodeIdentifier { get; private set; }

        private XBeeConfiguration(XBee xbee, XBeeAddress16 remoteXbee = null)
        {
            _xbee = xbee;
            _remoteXbee = remoteXbee;
        }

        /// <summary>
        /// Reads module basic information
        /// </summary>
        /// <param name="xbee">XBee module to read data from</param>
        /// <returns>XBee basic information</returns>
        public static XBeeConfiguration Read(XBee xbee)
        {
            return new XBeeConfiguration(xbee)
            {
                ApiMode = At.ApiMode.Read(xbee),
                HardwareVersion = At.HardwareVersion.Read(xbee),
                Firmware = At.Firmware.Read(xbee),
                SerialNumber = At.SerialNumber.Read(xbee),
                NodeIdentifier = At.NodeIdentifier.Read(xbee)
            };
        }

        /// <summary>
        /// Reads remote module basic information
        /// </summary>
        /// <param name="sender">XBee module that will send AT command to remote target</param>
        /// <param name="remoteXbee">XBee module which infomation will be retrieved</param>
        /// <returns>Remote XBee basic infomation</returns>
        public static XBeeConfiguration Read(XBee sender, XBeeAddress16 remoteXbee)
        {
            return new XBeeConfiguration(sender, remoteXbee)
            {
                ApiMode = At.ApiMode.Read(sender, remoteXbee),
                HardwareVersion = At.HardwareVersion.Read(sender, remoteXbee),
                Firmware = At.Firmware.Read(sender, remoteXbee),
                SerialNumber = At.SerialNumber.Read(sender, remoteXbee),
                NodeIdentifier = At.NodeIdentifier.Read(sender, remoteXbee)
            };
        }

        public void SetApiMode(ApiModes apiMode)
        {
            if (_remoteXbee != null)
            {
                At.ApiMode.Write(_xbee, _remoteXbee, apiMode);
            }
            else
            {
                At.ApiMode.Write(_xbee, apiMode);
            }

            ApiMode = apiMode;
        }

        public void SetNodeIdentifier(string nodeIdentifier)
        {
            if (_remoteXbee != null)
            {
                At.NodeIdentifier.Write(_xbee, _remoteXbee, nodeIdentifier);
            }
            else
            {
                At.NodeIdentifier.Write(_xbee, nodeIdentifier);
            }

            NodeIdentifier = nodeIdentifier;
        }

        /// <summary>
        /// Saves changes permanently in module flash memory.
        /// </summary>
        public void Save()
        {
            if (_remoteXbee != null)
            {
                SaveSettings.Write(_xbee, _remoteXbee);
            }
            else
            {
                SaveSettings.Write(_xbee);   
            }
        }

        public bool IsSeries1()
        {
            switch (HardwareVersion)
            {
                case HardwareVersions.SERIES1:
                case HardwareVersions.SERIES1_PRO:
                    return true;
                default:
                    return false;
            }
        }

        public bool IsSeries2()
        {
            switch (HardwareVersion)
            {
                case HardwareVersions.SERIES2:
                case HardwareVersions.SERIES2_PRO:
                case HardwareVersions.SERIES2B_PRO:
                    return true;
                default:
                    return false;
            }
        }

        public override string ToString()
        {
            return "ApiMode: " + At.ApiMode.GetName(ApiMode)
                   + ", HardwareVersion: " + At.HardwareVersion.GetName(HardwareVersion)
                   + ", Firmware: " + Firmware
                   + ", SerialNumber: " + SerialNumber
                   + ", NodeIdentifier: '" + NodeIdentifier + "'";
        }
    }
}