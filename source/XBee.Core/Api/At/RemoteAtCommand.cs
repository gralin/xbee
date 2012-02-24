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

        public RemoteAtCommand(string command, XBeeAddress64 remoteAddress64, XBeeAddress16 remoteAddress16, int[] value = null, int frameId = PacketIdGenerator.DefaultId, bool applyChanges = true)
            : this((AtCmd)UshortUtils.FromAscii(command), remoteAddress64, remoteAddress16, value, frameId, applyChanges)
        {
        }

        /// <summary>
        /// Creates a Remote AT request for setting an AT command on a remote XBee
        /// </summary>
        /// <remarks>
        /// Note: When setting a value, you must set applyChanges for the setting to
        /// take effect.  When sending several requests, you can wait until the last
        /// request before setting applyChanges=true.
        /// </remarks>
        /// <param name="command">two character AT command to set or query</param>
        /// <param name="remoteAddress64"></param>
        /// <param name="remoteAddress16"></param>
        /// <param name="value">if null then the current setting will be queried</param>
        /// <param name="frameId"></param>
        /// <param name="applyChanges">set to true if setting a value or issuing a command that changes the state of the radio (e.g. FR); not applicable to query requests</param>
        public RemoteAtCommand(AtCmd command, XBeeAddress64 remoteAddress64, XBeeAddress16 remoteAddress16, int[] value = null, int frameId = PacketIdGenerator.DefaultId, bool applyChanges = true) 
            : base(command, value, frameId)
        {
            RemoteAddress64 = remoteAddress64;
            RemoteAddress16 = remoteAddress16;
            ApplyChanges = applyChanges;
        }

        public RemoteAtCommand(string command, XBeeAddress64 remoteAddress64, int[] value = null)
            : this(command, remoteAddress64, XBeeAddress16.ZNET_BROADCAST, value)
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
        /// <param name="remoteAddress64"></param>
        /// <param name="value"></param>
        public RemoteAtCommand(AtCmd command, XBeeAddress64 remoteAddress64, int[] value = null)
            : this(command, remoteAddress64, XBeeAddress16.ZNET_BROADCAST, value)
        {
        }

        public RemoteAtCommand(string command, XBeeAddress16 remoteAddress16, int[] value = null)
            : this(command, XBeeAddress64.BROADCAST, remoteAddress16, value)
        {
        }

        /// <summary>
        /// Creates a Remote AT instance for setting the value of an AT command on a remote XBee, 
        /// by specifying the 16-bit address and value.  Uses the broadcast address for 64-bit address (00 00 00 00 00 00 ff ff)
        /// </summary>
        /// <remarks>
        /// Defaults are: frame id: 1, applyChanges: true
        /// </remarks>
        /// <param name="command"></param>
        /// <param name="remoteAddress16"></param>
        /// <param name="value"></param>
        public RemoteAtCommand(AtCmd command, XBeeAddress16 remoteAddress16, int[] value = null)
            : this(command, XBeeAddress64.BROADCAST, remoteAddress16, value)
        {
        }

        public override int[] GetFrameData()
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
            get { return ApiId.REMOTE_AT_REQUEST; }
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