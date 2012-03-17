namespace NETMF.OpenSource.XBee
{
    public interface IXBeeConnection
    {
        void Open();
        void Close();

        bool Connected { get; }

        void Send(byte[] data);
        void Send(byte[] data, int offset, int count);

        event DataReceivedEventHandler DataReceived;
    }

    public delegate void DataReceivedEventHandler(byte[] data, int offset, int count);
}