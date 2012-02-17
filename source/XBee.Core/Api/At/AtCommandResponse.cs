using System.Collections;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public class AtCommandResponse : XBeeFrameIdResponse
    {
        #region Status enum

        public enum Status
        {
            OK = 0,
            ERROR = 1,
            INVALID_COMMAND = 2,
            IVALID_PARAMETER = 3,

            /// <summary>
            /// Series 1 remote AT only according to spec.
            /// Also series 2 in 2x64 zb pro firmware
            /// </summary>
            NO_RESPONSE = 4
        }

        #endregion

        public AtCmd Command { get; protected set; }
        public string CommandName { get; protected set; }

        /// <summary>
        /// Returns the command data byte array.
        /// A zero length array will be returned if the command data is not specified.
        /// This is the case if the at command set a value, or executed a command that does
        /// not have a value (like FR)
        /// </summary>
        public Status ResponseStatus { get; protected set; }

        // response value msb to lsb
        public int[] Value { get; protected set; }

        public bool IsOk
        {
            get { return ResponseStatus == Status.OK; }
        }

        protected static readonly Hashtable Commands;

        static AtCommandResponse()
        {
            Commands = new Hashtable
            {
                {"WR", AtCmd.WR},    
                {"RE", AtCmd.RE},
                {"FR", AtCmd.FR},
                {"CH", AtCmd.CH},
                {"ID", AtCmd.ID},
                {"DH", AtCmd.DH},
                {"DL", AtCmd.DL},
                {"MY", AtCmd.MY},
                {"SH", AtCmd.SH},
                {"SL", AtCmd.SL},
                {"RR", AtCmd.RR},
                {"RN", AtCmd.RN},
                {"MM", AtCmd.MM},
                {"NI", AtCmd.NI},
                {"ND", AtCmd.ND},
                {"NT", AtCmd.NT},
                {"NO", AtCmd.NO},
                {"DN", AtCmd.DN},
                {"CE", AtCmd.CE},
                {"SC", AtCmd.SC},
                {"SD", AtCmd.SD},
                {"A1", AtCmd.A1},
                {"A2", AtCmd.A2},
                {"AI", AtCmd.AI},
                {"DA", AtCmd.DA},
                {"FP", AtCmd.FP},
                {"AS", AtCmd.AS},
                {"EE", AtCmd.EE},
                {"KY", AtCmd.KY},
                {"PL", AtCmd.PL},
                {"CA", AtCmd.CA},
                {"SM", AtCmd.SM},
                {"SO", AtCmd.SO},
                {"ST", AtCmd.ST},
                {"SP", AtCmd.SP},
                {"DP", AtCmd.DP},
                {"BD", AtCmd.BD},
                {"RO", AtCmd.RO},
                {"AP", AtCmd.AP},
                {"NB", AtCmd.NB},
                {"PR", AtCmd.PR},
                {"D7", AtCmd.D7},
                {"D6", AtCmd.D6},
                {"D5", AtCmd.D5},
                {"D4", AtCmd.D4},
                {"D3", AtCmd.D3},
                {"D2", AtCmd.D2},
                {"D1", AtCmd.D1},
                {"D0", AtCmd.D0},
                {"IU", AtCmd.IU},
                {"IT", AtCmd.IT},
                {"IS", AtCmd.IS},
                {"IO", AtCmd.IO},
                {"IC", AtCmd.IC},
                {"IR", AtCmd.IR},
                {"IA", AtCmd.IA},
                {"T0", AtCmd.T0},
                {"T1", AtCmd.T1},
                {"T2", AtCmd.T2},
                {"T3", AtCmd.T3},
                {"T4", AtCmd.T4},
                {"T5", AtCmd.T5},
                {"T6", AtCmd.T6},
                {"T7", AtCmd.T7},
                {"P0", AtCmd.P0},
                {"P1", AtCmd.P1},
                {"M0", AtCmd.M0},
                {"M1", AtCmd.M1},
                {"PT", AtCmd.PT},
                {"RP", AtCmd.RP},
                {"VR", AtCmd.VR},
                {"VL", AtCmd.VL},
                {"HV", AtCmd.HV},
                {"DB", AtCmd.DB},
                {"EC", AtCmd.EC},
                {"EA", AtCmd.EA},
                {"ED", AtCmd.ED},
                {"CT", AtCmd.CT},
                {"CN", AtCmd.CN},
                {"AC", AtCmd.AC},
                {"GT", AtCmd.GT},
                {"GC", AtCmd.GC},
            };
        }

        public override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("AT Response Frame Id");

            CommandName = Arrays.ToString(new[] { parser.Read("AT Response Char 1"), 
                                                  parser.Read("AT Response Char 2")});

            Command = (AtCmd)Commands[CommandName];
            ResponseStatus = (Status) parser.Read("AT Response Status");
            Value = parser.ReadRemainingBytes();
        }

        public override string ToString()
        {
            return "command=" + CommandName
                   + ",status=" + ResponseStatus
                   + ",value=" + (Value == null ? "null" : ByteUtils.ToBase16(Value))
                   + "," + base.ToString();
        }
    }
}