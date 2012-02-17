using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
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
    public class RemoteAtRequest : AtCommand
    {
        public XBeeAddress64 RemoteAddress64 { get; set; }
        public XBeeAddress16 RemoteAddress16 { get; set; }
        public bool ApplyChanges { get; set; }

        /// <summary>
        /// Creates a Remote AT request for setting an AT command on a remote XBee
        /// </summary>
        /// <remarks>
        /// Note: When setting a value, you must set applyChanges for the setting to
        /// take effect.  When sending several requests, you can wait until the last
        /// request before setting applyChanges=true.
        /// </remarks>
        /// <param name="frameId"></param>
        /// <param name="remoteAddress64"></param>
        /// <param name="remoteAddress16"></param>
        /// <param name="applyChanges">set to true if setting a value or issuing a command that changes the state of the radio (e.g. FR); not applicable to query requests</param>
        /// <param name="command">two character AT command to set or query</param>
        /// <param name="value">if null then the current setting will be queried</param>
        public RemoteAtRequest(int frameId, XBeeAddress64 remoteAddress64, XBeeAddress16 remoteAddress16, bool applyChanges, AtCmd command, int[] value) 
            : base(command, value)
        {
            FrameId = frameId;
            RemoteAddress64 = remoteAddress64;
            RemoteAddress16 = remoteAddress16;
            ApplyChanges = applyChanges;
        }

        /// <summary>
        /// Creates a Remote AT request for querying the current value of an AT command on a remote XBee
        /// </summary>
        /// <param name="frameId"></param>
        /// <param name="remoteAddress64"></param>
        /// <param name="remoteAddress16"></param>
        /// <param name="applyChanges"></param>
        /// <param name="command"></param>
        public RemoteAtRequest(int frameId, XBeeAddress64 remoteAddress64, XBeeAddress16 remoteAddress16, bool applyChanges, AtCmd command)
            : this(frameId, remoteAddress64, remoteAddress16, applyChanges, command, null)
        {
        }

        /// <summary>
        /// Abbreviated Constructor for setting an AT command on a remote XBee.
        /// This defaults to the DEFAULT_FRAME_ID, and true for apply changes
        /// </summary>
        /// <remarks>
        /// Note: the ZNET broadcast also works for series 1.  We could also use ffff but then that wouldn't work for series 2
        /// </remarks>
        /// <param name="dest64"></param>
        /// <param name="command"></param>
        /// <param name="value"></param>
        public RemoteAtRequest(XBeeAddress64 dest64, AtCmd command, int[] value)
            : this(DEFAULT_FRAME_ID, dest64, XBeeAddress16.ZNET_BROADCAST, true, command, value)
        {
            // apply changes doesn't make sense for a query
            ApplyChanges = false;
        }

        /// <summary>
        /// Creates a Remote AT instance for querying the value of an AT command on a remote XBee, 
        /// by specifying the 16-bit address.  Uses the broadcast address for 64-bit address (00 00 00 00 00 00 ff ff)
        /// </summary>
        /// <remarks>
        /// Defaults are: frame id: 1, applyChanges: true
        /// </remarks>
        /// <param name="dest16"></param>
        /// <param name="command"></param>
        public RemoteAtRequest(XBeeAddress16 dest16, AtCmd command)
            : this(dest16, command, null)
        {
            // apply changes doesn't make sense for a query
            ApplyChanges = false;
        }

        /// <summary>
        /// Creates a Remote AT instance for setting the value of an AT command on a remote XBee, 
        /// by specifying the 16-bit address and value.  Uses the broadcast address for 64-bit address (00 00 00 00 00 00 ff ff)
        /// </summary>
        /// <remarks>
        /// Defaults are: frame id: 1, applyChanges: true
        /// </remarks>
        /// <param name="remoteAddress16"></param>
        /// <param name="command"></param>
        /// <param name="value"></param>
        public RemoteAtRequest(XBeeAddress16 remoteAddress16, AtCmd command, int[] value)
            : this(DEFAULT_FRAME_ID, XBeeAddress64.BROADCAST, remoteAddress16, true, command, value)
        {
        }

        public override int[] GetFrameData()
        {
            var frameData = new OutputStream();

            // api id
            frameData.Write((byte)ApiId);
            // frame id (arbitrary byte that will be sent back with ack)
            frameData.Write(FrameId);

            frameData.Write(RemoteAddress64.GetAddress());
            frameData.Write(RemoteAddress16.GetAddress());

            // 0 - queue changes -- don't forget to send AC command
            frameData.Write(ApplyChanges ? 2 : 0);

            frameData.Write(Command[0]);
            frameData.Write(Command[1]);

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