namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketParser
    {
        int Read(string context);
        int[] ReadRemainingBytes();
        int GetFrameDataBytesRead();
        int GetRemainingBytes();
        int GetBytesRead();
        XBeePacketLength GetLength();
        ApiId GetApiId();
        int GetIntApiId();
	    // TODO move to util
	    XBeeAddress16 ParseAddress16();
	    XBeeAddress64 ParseAddress64();
    }
}