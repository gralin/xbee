﻿using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    /// <summary>
    /// API technique to set/query commands
    /// </summary>
    /// <remarks>
    /// WARNING: Any changes made will not survive a power cycle unless written to memory with WR command
    /// According to the manual, the WR command can only be written so many times.. however many that is.
    /// <para>
    /// API ID: 0x8
    /// </para>
    /// Determining radio type with HV:
    /// <list type="table">
    ///     <listheader>
    ///         <term>Byte 1</term>
    ///         <description>Part Number</description>
    ///     </listheader>  
    ///     <item>
    ///         <term>x17</term>
    ///         <description>XB24 (series 1)</description>
    ///     </item>
    ///     <item>
    ///         <term>x18</term>
    ///         <description>XBP24 (series 1)</description>
    ///     </item>
    ///     <item>
    ///         <term>x19</term>
    ///         <description>XB24-B (series 2</description>
    ///     </item>
    ///     <item>
    ///         <term>x1A</term>
    ///         <description>XBP24-B (series 2)</description>
    ///     </item>
    /// </list>
    /// XB24-ZB
    /// XBP24-ZB
    /// </remarks>
    public class AtCommand : XBeeRequest
    {
        public AtCmd Command { get; set; }
        public int[] Value { get; set; }

        public AtCommand(string command, int[] value = null, int frameId = DEFAULT_FRAME_ID)
            :this((AtCmd)UshortUtils.FromAscii(command), value, frameId)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <param name="frameId">frameId must be > 0 for a response</param>
        public AtCommand(AtCmd command, int[] value = null, int frameId = DEFAULT_FRAME_ID)
        {
            Command = command;
            Value = value;
            FrameId = frameId;
        }

        public AtCommand(string command, int value, int frameId = DEFAULT_FRAME_ID)
            : this(command, new[] { value }, frameId)
        {
        }

        public AtCommand(AtCmd command, int value, int frameId = DEFAULT_FRAME_ID)
            : this(command, new[] {value}, frameId)
        {
        }

        public override ApiId ApiId
        {
            get { return ApiId.AT_COMMAND; }
        }

        public override int[] GetFrameData()
        {
            var frameData = new OutputStream();

            frameData.Write((byte) ApiId);
            frameData.Write(FrameId);
            frameData.Write((ushort)Command);

            if (Value != null)
                frameData.Write(Value);

            return frameData.ToArray();
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",command=" + UshortUtils.ToAscii((ushort)Command)
                   + ",value=" + (Value == null ? "null" : ByteUtils.ToBase16(Value));
        }
    }
}