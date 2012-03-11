﻿namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Uses Remote AT to send a Force Sample (IS) AT command to a remote XBee
    /// </summary>
    public class ForceSampleRequest : At.RemoteAtCommand
    {
        /// <summary>
        /// Creates a Force Sample Remote AT request
        /// </summary>
        /// <param name="destination"></param>
        public ForceSampleRequest(XBeeAddress destination)
            : base((ushort) AtCmd.ForceSample, destination, null, false)
        {
        }
    }
}