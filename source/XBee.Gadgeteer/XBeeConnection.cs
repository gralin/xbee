using NETMF.OpenSource.XBee;

namespace Gadgeteer.Modules.GHIElectronics
{
    internal class XBeeConnection : IXBeeConnection
    {
        private readonly Interfaces.Serial _serial;
        private readonly byte[] _buffer;

        public XBeeConnection(Interfaces.Serial serial)
        {
            _buffer = new byte[1024];
            _serial = serial;

            _serial.DataReceived += (sender, data) =>
            {
                while (_serial.BytesToRead > 0)
                {
                    var bytesRead = _serial.Read(_buffer, 0, _serial.BytesToRead);

                    if (bytesRead <= 0)
                        return;

                    DataReceived(_buffer, 0, bytesRead);
                }
            };
        }

        public void Open()
        {
            _serial.Open();
        }

        public void Close()
        {
            _serial.Close();
        }

        public bool Connected
        {
            get { return _serial.IsOpen; }
        }

        public void Send(byte[] data)
        {
            _serial.Write(data, 0, data.Length);
        }

        public void Send(byte[] data, int offset, int count)
        {
            _serial.Write(data, offset, count);
        }

        public event DataReceivedEventHandler DataReceived;
    }
}