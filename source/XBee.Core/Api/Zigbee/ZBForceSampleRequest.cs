namespace Gadgeteer.Modules.GHIElectronics.Api.Zigbee
{
    /// <summary>
    /// Uses Remote AT to send a Force Sample (IS) AT command to a remote XBee
    /// </summary>
    public class ZBForceSampleRequest : RemoteAtRequest
    {
        /// <summary>
        /// Creates a Force Sample Remote AT request
        /// </summary>
        /// <param name="dest64"></param>
        public ZBForceSampleRequest(XBeeAddress64 dest64)
            : base(DEFAULT_FRAME_ID, dest64, XBeeAddress16.ZNET_BROADCAST, false, "IS", null)
        {
        }
    }
}