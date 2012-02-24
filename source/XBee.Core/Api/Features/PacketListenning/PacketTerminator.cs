﻿using System.Threading;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public abstract class PacketTerminator : IPacketTerminator
    {
        public ManualResetEvent Finished { get; set; }

        protected PacketTerminator()
        {
            Finished = new ManualResetEvent(false);
        }

        public abstract bool Terminate(XBeeResponse response);
    }
}