using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class SinglePacketListener : PacketListener
    {
        public SinglePacketListener(IPacketFilter filter)
            : base(filter)
        {
        }

        public XBeeResponse GetResponse(int timeout = -1)
        {
            while (!Finished)
            {
                var startTime = DateTime.Now;

                // wait max timeout time for any packet to be processed
                if (!PacketProcessed.WaitOne(timeout, false))
                    throw new XBeeTimeoutException();

                // -1 means timeout is not used
                if (timeout == -1)
                    continue;

                // decrement next iteration timeout value by elasped time
                timeout -= (int)(DateTime.Now.Subtract(startTime).Ticks / TimeSpan.TicksPerMillisecond);

                // end waiting if no timeout time is left
                if (timeout <= 0)
                    throw new XBeeTimeoutException();
            }

            return (XBeeResponse)Packets[0];
        }
    }
}