using System;
using System.Collections;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class PacketListener : IPacketListener
    {
        protected IPacketFilter Filter;
        protected readonly ArrayList Packets;
        protected AutoResetEvent PacketProcessed;

        public PacketListener(IPacketFilter filter)
        {
            Filter = filter;
            Packets = new ArrayList();
            PacketProcessed = new AutoResetEvent(false);
        }

        public bool Finished { get; protected set; }

        public void ProcessPacket(XBeeResponse packet)
        {
            if (Finished)
                return;

            if (Filter == null || Filter.Accepted(packet))
                Packets.Add(packet);

            Finished = (Filter != null && Filter.Finished());

            PacketProcessed.Set();
        }

        public XBeeResponse[] GetPackets(int timeout = -1)
        {
            if (Filter == null)
            {
                // if there is no Filter and timeout has been specified
                // it will cause the method to block for given timeout time
                if (timeout != -1)
                    Thread.Sleep(timeout);

                return GetPacketsAsArray();   
            }

            while (!Finished)
            {
                var startTime = DateTime.Now;

                // wait max timeout time for any packet to be processed
                if (!PacketProcessed.WaitOne(timeout, false))
                {
                    Logger.Debug("Packet listener failed to finish");
                    break;
                }

                // -1 means timeout is not used
                if (timeout == -1) 
                    continue;
                
                // decrement next iteration timeout value by elasped time
                timeout -= (int) (DateTime.Now.Subtract(startTime).Ticks/TimeSpan.TicksPerMillisecond);
                    
                // end waiting if no timeout time is left
                if (timeout <= 0)
                    break;
            }

            return GetPacketsAsArray();
        }

        protected XBeeResponse[] GetPacketsAsArray()
        {
            return (XBeeResponse[])Packets.ToArray(typeof(XBeeResponse));
        }
    }
}