﻿namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// Container for unknown response
    /// </summary>
    public class GenericResponse : XBeeResponse
    {
        public int GenericApiId { get; set; }

        protected override void Parse(IPacketParser parser)
        {
            //eat packet bytes -- they will be save to bytearray and stored in response
            parser.ReadRemainingBytes();
            // TODO gotta save it because it isn't know to the enum apiId won't
            GenericApiId = parser.GetIntApiId();	
        }
    }
}