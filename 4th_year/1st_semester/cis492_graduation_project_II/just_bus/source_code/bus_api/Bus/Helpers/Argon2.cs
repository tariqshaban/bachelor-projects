using Konscious.Security.Cryptography;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace Bus.Helpers
{
    public class Argon2
    {
        private static byte[] CreateSalt()
        {
            var buffer = new byte[16];
            var rng = new RNGCryptoServiceProvider();
            rng.GetBytes(buffer);

            return buffer;
        }

        private static byte[] HashPasswordWithSalt(string password, string salt)
        {
            var argon2id = new Argon2id(StringToUTF8(password))
            {
                Salt = Base64ToBytes(salt),
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 5120
            };

            return argon2id.GetBytes(16);
        }

        public static HashedPassword HashPassword(string password)
        {
            byte[] salt = CreateSalt();

            var argon2id = new Argon2id(StringToUTF8(password))
            {
                Salt = salt,
                DegreeOfParallelism = 8,
                Iterations = 4,
                MemorySize = 5120
            };

            return new HashedPassword(BytesToBase64(argon2id.GetBytes(16)), BytesToBase64(salt));
        }

        public static bool VerifyHash(string password, string hash, string salt)
        {
            if (!IsBase64String(hash) || !IsBase64String(salt))
            {
                return false;
            }

            var newHash = BytesToBase64(HashPasswordWithSalt(password, salt));

            return hash.SequenceEqual(newHash);
        }

        private static byte[] StringToUTF8(string plainText)
        {
            return System.Text.Encoding.UTF8.GetBytes(plainText);
        }

        private static byte[] Base64ToBytes(string base64Text)
        {
            return Convert.FromBase64String(base64Text);
        }

        private static string BytesToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        private static bool IsBase64String(string base64)
        {
            Span<byte> buffer = new(new byte[base64.Length]);
            return Convert.TryFromBase64String(base64, buffer, out _);
        }
    }
}