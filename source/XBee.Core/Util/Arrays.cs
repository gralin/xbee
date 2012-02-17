using System;
using System.Text;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public static class Arrays
    {
        public static bool AreEqual(int[] array1, int[] array2)
        {
            throw new NotImplementedException();
        }

        public static int HashCode(int[] array)
        {
            throw new NotImplementedException();
        }

        public static byte[] ToByteArray(int[] array)
        {
            var result = new byte[array.Length];

            for (var i = 0; i < array.Length; i++)
                result[i] = (byte) array[i];

            return result;
        }

        public static int[] ToIntArray(byte[] array)
        {
            var result = new int[array.Length];

            for (var i = 0; i < array.Length; i++)
                result[i] = array[i];

            return result;
        }

        public static int[] ToIntArray(string message)
        {
            return ToIntArray(Encoding.UTF8.GetBytes(message));
        }

        public static string ToString(int[] array)
        {
            return new string(Encoding.UTF8.GetChars(ToByteArray(array)));
        }
    }
}