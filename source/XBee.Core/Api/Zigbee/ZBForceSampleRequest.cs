using Gadgeteer.Modules.GHIElectronics.Api.At;

namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Uses Remote AT to send a Force Sample (IS) AT command to a remote XBee
    /// </summary>
    public class ZBForceSampleRequest : RemoteAtCommand
    {
        /// <summary>
        /// Creates a Force Sample Remote AT request
        /// </summary>
        /// <param name="destination"></param>
        public ZBForceSampleRequest(XBeeAddress destination)
            : base(AtCmd.ForceSample, destination, null, PacketIdGenerator.DefaultId, false)
        {
        }
    }
}