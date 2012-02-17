using System.IO;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class OutputStream : IOutputStream
    {
        private readonly Stream _stream;

        public OutputStream()
        {
            _stream = new MemoryStream();
        }

        #region IOutputStream Members

        public void Write(byte data)
        {
            _stream.WriteByte(data);
        }

        public void Write(int data)
        {
            _stream.WriteByte((byte) data);
        }

        public void Write(byte[] data)
        {
            _stream.Write(data, 0, data.Length);
        }

        public void Write(int[] data)
        {
            Write(Arrays.ToByteArray(data));
        }

        public int[] ToArray()
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

        #endregion
    }
}