namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IXBee
    {
        void Open();

        void AddPacketListener(IPacketListener packetListener);

        void RemovePacketListener(IPacketListener packetListener);

        void SendPacket(XBeePacket packet);

        void SendPacket(int[] packet);

        void SendAsync(XBeeRequest xbeeRequest);

        XBeeResponse Send(XBeeRequest xbeeRequest, int timeout);

        XBeeResponse GetResponse();

        XBeeResponse GetResponse(int timeout);

        void Close();

        int GetCurrentFrameId();

        int GetNextFrameId();

        void UpdateFrameId(int val);

        bool IsConnected();

        void ClearResponseQueue();

        XBeeResponse[] CollectResponses(int wait, int maxPacketCount);

        XBeeResponse[] CollectResponses(int wait, ICollectTerminator terminator);
    }
}