using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public interface IInputStream : IDisposable
    {
        int Read();
        int Read(string message);
        int[] Read(int count);
    }
}