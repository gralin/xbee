using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    // TODO: Change everywhere to use byte arrays instead of integer arrays
    public interface IIntArray : IDisposable
    {
        int[] GetIntArray();
    }
}