using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class SinglePacketListener : PacketListener
    {
        public SinglePacketListener(Type expectedPacketType = null)
        {
            Validator = expectedPacketType != null ? new TypeValidator(expectedPacketType) : null;
            Terminator = new CountLimitTerminator(1);
        }

        public XBeeResponse GetPacket(int timeout = -1)
        {
            if (!Terminator.Finished.WaitOne(timeout, false))
                throw new XBeeTimeoutException();

            return (XBeeResponse) Packets[0];
        }
    }
}