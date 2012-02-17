using System;
using System.IO;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class InputStream : IInputStream
    {
        private readonly Stream _stream;

        public InputStream(Stream stream)
        {
            _stream = stream;
            _stream.Position = 0;
        }

        public InputStream(int[] source)
        {
            _stream = new MemoryStream(Arrays.ToByteArray(source));
        }

        public InputStream(byte[] source)
        {
            _stream = new MemoryStream(source);
        }

        #region IInputStream Members

        public int Read()
        {
            int result = _stream.ReadByte();

            if (result == -1)
                throw new InvalidOperationException("end of input stream");

            return result;
        }

        public int Read(string message)
        {
            return Read();
        }

        /// <summary>
        /// Reads <paramref name="count"/> bytes from the input stream and returns the bytes in an array
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public int[] Read(int count)
        {
            var block = new byte[count];
            _stream.Read(block, 0, count);
            return Arrays.ToIntArray(block);
        }

        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();
        }

        #endregion
    }
}