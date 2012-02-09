using System.IO;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public class IntArrayOutputStream : IIntArray
    {
        private readonly MemoryStream _stream;

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
            _stream.Position = 0;
            var result = new byte[_stream.Length];
            _stream.Read(result, 0, result.Length);
            var resultAsIntArray = new int[result.Length];

            for (var i = 0; i < result.Length; i++)
                resultAsIntArray[i] = result[i];

            return resultAsIntArray;
        }

        public void Dispose()
        {
            _stream.Dispose();
        }
    }
}