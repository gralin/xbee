﻿using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api.At
{
    public class AtResponse : XBeeFrameIdResponse
    {
        public AtCmd Command { get; protected set; }

        /// <summary>
        /// Returns the command data byte array.
        /// A zero length array will be returned if the command data is not specified.
        /// This is the case if the at command set a value, or executed a command that does
        /// not have a value (like FR)
        /// </summary>
        public AtResponseStatus Status { get; protected set; }

        // response value msb to lsb
        public int[] Value { get; protected set; }

        public bool IsOk
        {
            get { return Status == AtResponseStatus.OK; }
        }

        public override void Parse(IPacketParser parser)
        {
            FrameId = parser.Read("AT Response Frame Id");

            Command = (AtCmd)UshortUtils.ToUshort(
                parser.Read("AT Response Char 1"), 
                parser.Read("AT Response Char 2"));

            Status = (AtResponseStatus) parser.Read("AT Response Status");
            Value = parser.ReadRemainingBytes();
        }

        public override string ToString()
        {
            return "command=" + UshortUtils.ToAscii((ushort)Command)
                   + ",status=" + Status
                   + ",value=" + (Value == null ? "null" : ByteUtils.ToBase16(Value))
                   + "," + base.ToString();
        }
    }
}