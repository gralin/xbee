using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class IntArrayInputStream : IIntInputStream
    {
        private readonly int[] _source;
        private int _pos;

        public IntArrayInputStream(int[] source)
        {
            _source = source;
        }

        public int Read()
        {
            if (_pos >= _source.Length)
                throw new InvalidOperationException("end of input stream");

            return _source[_pos++];
        }

        /// <summary>
        /// Reads <paramref name="size"/> bytes from the input stream and returns the bytes in an array
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public int[] Read(int size)
        {
            var block = new int[size];
            Array.Copy(_source, _pos, block, 0, size);
            // index pos
            _pos += size;
            return block;
        }

        public int Read(string s)
        {
            return Read();
        }
    }
}