using System;
using System.Collections;
using System.Threading;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class MultiplePacketListener : IPacketListener
    {
        private readonly ICollectTerminator _terminator;
        private readonly ArrayList _packets;
        private readonly AutoResetEvent _packetsReceived;
        private readonly Type _expectedPacketType;

        public MultiplePacketListener(Type expectedPacketType = null, ICollectTerminator terminator = null)
        {
            _expectedPacketType = expectedPacketType;
            _terminator = terminator;
            _packets = new ArrayList();
            _packetsReceived = new AutoResetEvent(false);
        }

        public void ProcessPacket(XBeeResponse response)
        {
            if (_expectedPacketType == null || _expectedPacketType.IsInstanceOfType(response))
                _packets.Add(response);

            if (_terminator != null && _terminator.Stop(response))
                _packetsReceived.Set();
        }

        public XBeeResponse[] GetPackets(int timeout)
        {
            if (!_packetsReceived.WaitOne(timeout, false) && _terminator != null)
                return new XBeeResponse[0];

            var result = new XBeeResponse[_packets.Count];

            for (var i = 0; i < _packets.Count; i++)
                result[i] = (XBeeResponse) _packets[i];

            return result;
        }
    }
}