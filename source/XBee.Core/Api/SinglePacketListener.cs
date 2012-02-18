using System;
using System.Threading;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class SinglePacketListener : IPacketListener
    {
        private readonly Type _expectedPacketType;
        private XBeeResponse _expectedPacket;
        private readonly AutoResetEvent _packetReceived;

        public SinglePacketListener(Type expectedPacketType = null)
        {
            _expectedPacketType = expectedPacketType;
            _packetReceived = new AutoResetEvent(false);
        }

        public void ProcessPacket(XBeeResponse response)
        {
            if (_expectedPacket != null)
                return;

            if (_expectedPacketType != null && !_expectedPacketType.IsInstanceOfType(response)) 
                return;

            _expectedPacket = response;
            _packetReceived.Set();
        }

        public XBeeResponse GetPacket(int timeout)
        {
            if (!_packetReceived.WaitOne(timeout, false))
                throw new XBeeTimeoutException();

            return _expectedPacket;
        }
    }
}