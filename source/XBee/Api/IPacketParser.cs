namespace NETMF.OpenSource.XBee.Api
{
    public interface IPacketParser
    {
        byte Read();
        byte Read(string context);
        byte[] ReadRemainingBytes();
        int FrameDataBytesRead { get; }
        int RemainingBytes { get; }
        int BytesRead { get; }
        ushort Length { get; }
        ApiId ApiId { get; }
	    XBeeAddress16 ParseAddress16();
	    XBeeAddress64 ParseAddress64();
    }
}