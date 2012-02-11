using System;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    /// <summary>
    /// This is an API for communicating with Digi XBee 802.15.4 and ZigBee radios
    /// </summary>
    public class XBee : IXBee
    {
        private object _sendPacketBlock;
        private readonly IXBeeConnection _connection;

        public XBee(IXBeeConnection connection)
        {
            _connection = connection;
            _connection.DataReceived += OnDataReceived;
        }

        public XBee(string portName, int baudRate)
        {
            _connection = new SerialConnection(portName, baudRate);
            _connection.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(byte[] data, int offset, int count)
        {
            throw new NotImplementedException();
        }

        #region IXBee Members

        public void Open()
        {
            _connection.Open();
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