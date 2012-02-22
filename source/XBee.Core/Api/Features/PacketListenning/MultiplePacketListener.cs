using System;

namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
{
    public class MultiplePacketListener : PacketListener
    {
        public MultiplePacketListener(int maxPacketCount = int.MaxValue, Type expectedPacketType = null)
        {
            Validator = expectedPacketType != null ? new TypeValidator(expectedPacketType) : null;
            Terminator = maxPacketCount != int.MaxValue ? new CountLimitTerminator(maxPacketCount) : null; 
        }
    }
}