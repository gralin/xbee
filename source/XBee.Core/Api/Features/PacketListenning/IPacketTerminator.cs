using System.Threading;

namespace Gadgeteer.Modules.GHIElectronics.Api.Features.PacketListenning
{
    public interface IPacketTerminator
    {
        ManualResetEvent Finished { get; }
        bool Terminate(XBeeResponse response);
    }
}