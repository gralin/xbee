using System.IO.Ports;

namespace NETMF.OpenSource.XBee
{
    internal class SerialConnection : IXBeeConnection
    {
        private readonly SerialPort _serialPort;
        private readonly byte[] _buffer;

        public SerialConnection(string portName, int baudRate)
        {
            _serialPort = new SerialPort(portName, baudRate);
            _buffer = new byte[1024];

            _serialPort.DataReceived += (sender, args) =>
            {
                while (_serialPort.BytesToRead > 0)
                {
                    var bytesRead = _serialPort.Read(_buffer, 0, _serialPort.BytesToRead);

                    if (bytesRead <= 0)
                        return;

                    DataReceived(_buffer, 0, bytesRead);
                }
            };
        }

        public void Open()
        {
            _serialPort.Open();
        }

        public void Close()
        {
            _serialPort.Close();
        }

        public bool Connected
        {
            get { return _serialPort.IsOpen; }
        }

        public void Send(byte[] data)
        {
            _serialPort.Write(data, 0, data.Length);
        }

        public void Send(byte[] data, int offset, int count)
        {
            _serialPort.Write(data, offset, count);
        }

        public event DataReceivedEventHandler DataReceived;
    }
}