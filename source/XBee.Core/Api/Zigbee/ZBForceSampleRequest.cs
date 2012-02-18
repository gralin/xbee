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
        /// <param name="dest64"></param>
        public ZBForceSampleRequest(XBeeAddress64 dest64)
            : base(AtCmd.ForceSample, dest64, XBeeAddress16.ZNET_BROADCAST, null, DEFAULT_FRAME_ID, false)
        {
        }
    }
}