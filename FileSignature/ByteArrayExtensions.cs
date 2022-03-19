using System;

namespace Signature
{
    public static class ByteArrayExtensions
    {
        public static string ConvertToString(this byte[] bytes)
        {
            return BitConverter.ToString(bytes);
        }
    }
}