using System.IO;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class IntArrayOutputStream : IIntArray
    {
        private readonly Stream _stream;

        public IntArrayOutputStream()
        {
            _stream = new MemoryStream();
        }

        public void Write(byte val)
        {
            _stream.WriteByte(val);
        }

        public void Write(int val)
        {
            _stream.WriteByte((byte)val);
        }

        public void Write(byte[] val)
        {
            _stream.Write(val, 0, val.Length);
        }

        public void Write(int[] val)
        {
            foreach (var i in val)
                _stream.WriteByte((byte)i);
        }

        public int[] GetIntArray()
        {
            var result = new byte[_stream.Length];
            _stream.Position = 0;
            _stream.Read(result, 0, result.Length);

            return Arrays.ToIntArray(result);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}