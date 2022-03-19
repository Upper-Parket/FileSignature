using System.Security.Cryptography;

namespace Signature
{
    public static class HashGenerator
    {
        public static byte[] Generate(byte[] bytes)
        {
            using (var sha = SHA256.Create())
            {
                return sha.ComputeHash(bytes);
            }
        }
    }
}