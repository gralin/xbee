using System;

namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
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
            if (Terminator == null)
            {
                if (timeout != -1)
                    throw new InvalidOperationException("Timeout can't be used without terminator provided");

                return (XBeeResponse)(Packets.Count > 0 ? Packets[0] : null);   
            }

            if (!Terminator.Finished.WaitOne(timeout, false))
                throw new XBeeTimeoutException();

            return (XBeeResponse)Packets[0];
        }
    }
}