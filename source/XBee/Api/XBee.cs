using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public class XBee : IXBee
    {
        #region IXBee Members

        public void Open(string port, int baudRate)
        {
            throw new NotImplementedException();
        }

        public void AddPacketListener(IPacketListener packetListener)
        {
            throw new NotImplementedException();
        }

        public void RemovePacketListener(IPacketListener packetListener)
        {
            throw new NotImplementedException();
        }

        public void SendPacket(XBeePacket packet)
        {
            throw new NotImplementedException();
        }

        public void SendPacket(int[] packet)
        {
            throw new NotImplementedException();
        }

        public void SendAsynchronous(XBeeRequest xbeeRequest)
        {
            throw new NotImplementedException();
        }

        public XBeeResponse SendSynchronous(XBeeRequest xbeeRequest, int timeout)
        {
            throw new NotImplementedException();
        }

        public XBeeResponse GetResponse()
        {
            throw new NotImplementedException();
        }

        public XBeeResponse GetResponse(int timeout)
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public int GetCurrentFrameId()
        {
            throw new NotImplementedException();
        }

        public int GetNextFrameId()
        {
            throw new NotImplementedException();
        }

        public void UpdateFrameId(int val)
        {
            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public void ClearResponseQueue()
        {
            throw new NotImplementedException();
        }

        public XBeeResponse[] CollectResponses(int wait, ICollectTerminator terminator)
        {
            throw new NotImplementedException();
        }

        #endregion

        public string SayHello()
        {
            return "Hello from XBee";
        }
    }
}