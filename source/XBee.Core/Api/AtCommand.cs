using System;
using System.Collections;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
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
        public string Command { get; set; }
        public int[] Value { get; set; }

        private static readonly Hashtable CommandNames;

        static AtCommand()
        {
            CommandNames = new Hashtable
            {
                {AtCmd.WR, "WR"},    
                {AtCmd.RE, "RE"},
                {AtCmd.FR, "FR"},
                {AtCmd.CH, "CH"},
                {AtCmd.ID, "ID"},
                {AtCmd.DH, "DH"},
                {AtCmd.DL, "DL"},
                {AtCmd.MY, "MY"},
                {AtCmd.SH, "SH"},
                {AtCmd.SL, "SL"},
                {AtCmd.RR, "RR"},
                {AtCmd.RN, "RN"},
                {AtCmd.MM, "MM"},
                {AtCmd.NI, "NI"},
                {AtCmd.ND, "ND"},
                {AtCmd.NT, "NT"},
                {AtCmd.NO, "NO"},
                {AtCmd.DN, "DN"},
                {AtCmd.CE, "CE"},
                {AtCmd.SC, "SC"},
                {AtCmd.SD, "SD"},
                {AtCmd.A1, "A1"},
                {AtCmd.A2, "A2"},
                {AtCmd.AI, "AI"},
                {AtCmd.DA, "DA"},
                {AtCmd.FP, "FP"},
                {AtCmd.AS, "AS"},
                {AtCmd.EE, "EE"},
                {AtCmd.KY, "KY"},
                {AtCmd.PL, "PL"},
                {AtCmd.CA, "CA"},
                {AtCmd.SM, "SM"},
                {AtCmd.SO, "SO"},
                {AtCmd.ST, "ST"},
                {AtCmd.SP, "SP"},
                {AtCmd.DP, "DP"},
                {AtCmd.BD, "BD"},
                {AtCmd.RO, "RO"},
                {AtCmd.AP, "AP"},
                {AtCmd.NB, "NB"},
                {AtCmd.PR, "PR"},
                {AtCmd.D7, "D7"},
                {AtCmd.D6, "D6"},
                {AtCmd.D5, "D5"},
                {AtCmd.D4, "D4"},
                {AtCmd.D3, "D3"},
                {AtCmd.D2, "D2"},
                {AtCmd.D1, "D1"},
                {AtCmd.D0, "D0"},
                {AtCmd.IU, "IU"},
                {AtCmd.IT, "IT"},
                {AtCmd.IS, "IS"},
                {AtCmd.IO, "IO"},
                {AtCmd.IC, "IC"},
                {AtCmd.IR, "IR"},
                {AtCmd.IA, "IA"},
                {AtCmd.T0, "T0"},
                {AtCmd.T1, "T1"},
                {AtCmd.T2, "T2"},
                {AtCmd.T3, "T3"},
                {AtCmd.T4, "T4"},
                {AtCmd.T5, "T5"},
                {AtCmd.T6, "T6"},
                {AtCmd.T7, "T7"},
                {AtCmd.P0, "P0"},
                {AtCmd.P1, "P1"},
                {AtCmd.M0, "M0"},
                {AtCmd.M1, "M1"},
                {AtCmd.PT, "PT"},
                {AtCmd.RP, "RP"},
                {AtCmd.VR, "VR"},
                {AtCmd.VL, "VL"},
                {AtCmd.HV, "HV"},
                {AtCmd.DB, "DB"},
                {AtCmd.EC, "EC"},
                {AtCmd.EA, "EA"},
                {AtCmd.ED, "ED"},
                {AtCmd.CT, "CT"},
                {AtCmd.CN, "CN"},
                {AtCmd.AC, "AC"},
                {AtCmd.GT, "GT"},
                {AtCmd.GC, "GC"},
            };
        }

        public AtCommand(AtCmd command)
            : this(command, null, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(AtCmd command, int value)
            : this(command, new[] {value}, DEFAULT_FRAME_ID)
        {
        }

        public AtCommand(AtCmd command, int[] value)
            : this(command, value, DEFAULT_FRAME_ID)
        {
        }

        /// <summary>
        /// </summary>
        /// <param name="command"></param>
        /// <param name="value"></param>
        /// <param name="frameId">frameId must be > 0 for a response</param>
        public AtCommand(AtCmd command, int[] value, int frameId)
        {
            Command = (string) CommandNames[command];
            Value = value;
            FrameId = frameId;
        }

        public override ApiId ApiId
        {
            get { return ApiId.AT_COMMAND; }
        }

        public override int[] GetFrameData()
        {
            if (Command.Length > 2)
                throw new ArgumentException("Command should be two characters. Do not include AT prefix");

            var frameData = new OutputStream();

            frameData.Write((byte) ApiId);
            frameData.Write(FrameId);
            frameData.Write(Command);

            if (Value != null)
                frameData.Write(Value);

            return frameData.ToArray();
        }

        public override string ToString()
        {
            return base.ToString()
                   + ",command=" + Command
                   + ",value=" + (Value == null ? "null" : ByteUtils.ToBase16(Value));
        }
    }
}