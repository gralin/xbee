using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class MultiplePacketListener : PacketListener
    {
        public MultiplePacketListener(byte maxPacketCount = byte.MaxValue, Type expectedPacketType = null)
        {
            Validator = expectedPacketType != null ? new TypeValidator(expectedPacketType) : null;
            Terminator = maxPacketCount != byte.MaxValue ? new CountLimitTerminator(maxPacketCount) : null; 
        }
    }
}