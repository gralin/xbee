namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketParser
    {
        int Read();
        int Read(string context);
        int[] ReadRemainingBytes();
        int FrameDataBytesRead { get; }
        int RemainingBytes { get; }
        int BytesRead { get; }
        ushort Length { get; }
        ApiId ApiId { get; }
	    XBeeAddress16 ParseAddress16();
	    XBeeAddress64 ParseAddress64();
    }
}