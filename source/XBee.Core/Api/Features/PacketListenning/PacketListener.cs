using System;
using System.Collections;
using System.Threading;
using Gadgeteer.Modules.GHIElectronics.Util;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class PacketListener : IPacketListener
    {
        protected IPacketValidator Validator;
        protected IPacketTerminator Terminator;
        protected readonly ArrayList Packets;
        public AutoResetEvent PacketAccepted { get; set; }

        public PacketListener()
        {
            PacketAccepted = new AutoResetEvent(false);
            Packets = new ArrayList();
        }

        public PacketListener(IPacketTerminator terminator = null, IPacketValidator validator = null)
            : this()
        {
            Validator = validator;
            Terminator = terminator;
        }

        public PacketListener(IPacketValidator validator = null, IPacketTerminator terminator = null)
            : this(terminator, validator)
        {
        }

        public void ProcessPacket(XBeeResponse packet)
        {
            if (Validator != null && !Validator.Validate(packet))
                return;
            
            if (Terminator != null && Terminator.Terminate(packet))
                return;

            Packets.Add(packet);
            PacketAccepted.Set();
        }

        public XBeeResponse[] GetPackets(int timeout = -1)
        {
            if (Terminator == null)
            {
                if (timeout != -1)
                    throw new InvalidOperationException("Timeout can't be used without terminator provided");

                return GetPacketsAsArray();   
            }

            if (!Terminator.Finished.WaitOne(timeout, false))
                Logger.Debug("Failed to receive expected number of responses");

            return GetPacketsAsArray();
        }

        protected XBeeResponse[] GetPacketsAsArray()
        {
            var responses = new XBeeResponse[Packets.Count];

            for (var i = 0; i < responses.Length; i++)
                responses[i] = (XBeeResponse) Packets[i];

            return responses;
        }
    }
}