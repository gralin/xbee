using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
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

        public int Char1 { get; protected set; }
        public int Char2 { get; protected set; }

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

        public string GetCommand()
        {
            return new string(new[] {(char) Char1, (char) Char2});
        }

        protected override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("AT Response Frame Id");
            Char1 = parser.Read("AT Response Char 1");
            Char2 = parser.Read("AT Response Char 2");
            ResponseStatus = (Status) parser.Read("AT Response Status");
            Value = parser.ReadRemainingBytes();
        }

        public override string ToString()
        {
            return "command=" + GetCommand()
                   + ",status=" + ResponseStatus
                   + ",value=" + (Value == null ? "null" : ByteUtils.ToBase16(Value))
                   + "," + base.ToString();
        }
    }
}