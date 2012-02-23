using System.Threading;

namespace Gadgeteer.Modules.GHIElectronics.Api
{
    public interface IPacketTerminator
    {
        ManualResetEvent Finished { get; }
        bool Terminate(XBeeResponse response);
    }
}