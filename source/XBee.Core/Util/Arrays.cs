using System.Text;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public static class Arrays
    {
        public static bool AreEqual(int[] array1, int[] array2)
        {
            if (array1.Length != array2.Length)
                return false;

            for (var i = 0; i < array1.Length; i++)
                if (array1[i] != array2[i])
                    return false;

            return true;
        }

        public static int HashCode(int[] array)
        {
            var result = array.Length;

            for (var i = 0; i < array.Length; i++)
                if (array[i] > 0)
                    result *= (array[i] << i);

            return result;
        }

        public static byte[] ToByteArray(int[] array)
        {
            var result = new byte[array.Length];

            for (var i = 0; i < array.Length; i++)
                result[i] = (byte) array[i];

            return result;
        }

        public static byte[] ToByteArray(string value)
        {
            return Encoding.UTF8.GetBytes(value);
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
            return ToIntArray(ToByteArray(message));
        }

        public static int[] ToIntArray(ushort value)
        {
            return new[] { UshortUtils.Msb(value), UshortUtils.Lsb(value) };
        }

        public static string ToString(int[] array)
        {
            return new string(Encoding.UTF8.GetChars(ToByteArray(array)));
        }
    }
}