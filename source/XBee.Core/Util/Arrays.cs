using System;

namespace Gadgeteer.Modules.GHIElectronics.Util
{
    public static class Arrays
    {
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
    }
}