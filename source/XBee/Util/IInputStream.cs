using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public interface IInputStream : IDisposable
    {
        byte Read();
        byte Read(string message);
        byte[] Read(int count);
    }
}