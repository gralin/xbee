using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public interface IIntInputStream : IDisposable
    {
        int Read();
        int Read(string s);
    }
}