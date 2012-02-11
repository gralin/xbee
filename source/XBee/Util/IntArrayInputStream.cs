using System;
using System.IO;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class IntArrayInputStream : IIntInputStream
    {
        private readonly Stream _stream;

        public IntArrayInputStream(Stream stream)
        {
            _stream = stream;
            _stream.Position = 0;
        }

        public IntArrayInputStream(int[] source)
        {
            _stream = new MemoryStream(Arrays.ToByteArray(source));
        }

        public int Read()
        {
            var result = _stream.ReadByte();

            if (result == -1)
                throw new InvalidOperationException("end of input stream");

            return result;
        }

        /// <summary>
        /// Reads <paramref name="size"/> bytes from the input stream and returns the bytes in an array
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public int[] Read(int size)
        {
            var block = new byte[size];
            _stream.Read(block, 0, size);

            return Arrays.ToIntArray(block);
        }

        public int Read(string s)
        {
            return Read();
        }

        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();
        }
    }
}