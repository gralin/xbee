using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// Supported by both series 1 (10C8 firmware and later) and series 2.
    /// Allows AT commands to be sent to a remote radio.
    /// API ID: 0x17
    /// </summary>
    /// <remarks>
    /// Warning: this command may not return a response if the remote radio is unreachable.
    /// You will need to set your own timeout when waiting for a response from this command,
    /// or you may wait forever.
    /// </remarks>
    public class RemoteAtCommand : AtCommand
    {
        public XBeeAddress64 RemoteAddress64 { get; set; }
        public XBeeAddress16 RemoteAddress16 { get; set; }
        public bool ApplyChanges { get; set; }

        public RemoteAtCommand(string command, XBeeAddress remoteAddress, byte[] value = null, bool applyChanges = true)
            : this((AtCmd)UshortUtils.FromAscii(command), remoteAddress, value, applyChanges)
        {
        }

        /// <summary>
        /// Abbreviated Constructor for setting an AT command on a remote XBee.
        /// This defaults to the DefaultId, and true for apply changes
        /// </summary>
        /// <remarks>
        /// Note: the ZNET broadcast also works for series 1.  We could also use ffff but then that wouldn't work for series 2
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="remoteAddress"></param>
        /// <param name="value"></param>
        /// <param name="frameId"> </param>
        /// <param name="applyChanges"> </param>
        public RemoteAtCommand(AtCmd command, XBeeAddress remoteAddress, byte[] value = null, bool applyChanges = true)
            : base(command, value)
        {
            if (remoteAddress is XBeeAddress16)
            {
                RemoteAddress16 = (XBeeAddress16)remoteAddress;
                RemoteAddress64 = XBeeAddress64.Broadcast;
            }
            else
            {
                RemoteAddress16 = XBeeAddress16.ZnetBroadcast;
                RemoteAddress64 = (XBeeAddress64)remoteAddress;
            }

            ApplyChanges = applyChanges;
        }

        public override byte[] GetFrameData()
        {
            var frameData = new OutputStream();

            // api id
            frameData.Write((byte)ApiId);
            // frame id (arbitrary byte that will be sent back with ack)
            frameData.Write(FrameId);

            frameData.Write(RemoteAddress64.Address);
            frameData.Write(RemoteAddress16.Address);

            // 0 - queue changes -- don't forget to send AC command
            frameData.Write(ApplyChanges ? 2 : 0);

            frameData.Write((ushort)Command);

            if (Value != null)
                frameData.Write(Value);

            return frameData.ToArray();
        }

        public override ApiId ApiId
        {
            get { return ApiId.RemoteAtRequest; }
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",remoteAddr64=" + RemoteAddress64
                   + ",remoteAddr16=" + RemoteAddress16
                   + ",applyChanges=" + ApplyChanges;
        }
    }
}