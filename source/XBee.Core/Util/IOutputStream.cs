using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    // TODO: Change everywhere to use byte arrays instead of integer arrays
    public interface IOutputStream : IDisposable
    {
        void Write(byte data);
        void Write(int data);
        void Write(ushort data);
        void Write(string data);
        void Write(byte[] data);
        void Write(int[] data);
        int[] ToArray();
    }
}