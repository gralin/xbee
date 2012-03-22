using System.IO.Ports;
using System.Threading;
using GHIElectronics.NETMF.USBHost;
using NETMF.OpenSource.XBee;

namespace NETMF.Tester
{
    internal class UsbHostConnection : IXBeeConnection
    {
        private readonly byte[] _buffer;
        private readonly Thread _readThread;
        private readonly USBH_SerialUSB _serial;

        public UsbHostConnection(USBH_Device device, USBH_DeviceType deviceType = USBH_DeviceType.Serial_FTDI)
        {
            _buffer = new byte[1024];

            var usbhDevice = new USBH_Device(device.ID, device.INTERFACE_INDEX, deviceType, device.VENDOR_ID,
                                             device.PRODUCT_ID, device.PORT_NUMBER);

            _serial = new USBH_SerialUSB(usbhDevice, 9600, Parity.None, 8, StopBits.One);

            _readThread = new Thread(() =>
            {
                while (Connected)
                {
                    var bytesRead = _serial.Read(_buffer, 0, _buffer.Length);

                    if (bytesRead > 0)
                        DataReceived(_buffer, 0, bytesRead);
                }
            });
        }

        #region IXBeeConnection Members

        public bool Connected { get; private set; }

        public void Open()
        {
            Connected = true;
            _readThread.Start();
        }

        public void Close()
        {
            Connected = false;
            _readThread.Join();
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

        #endregion
    }
}