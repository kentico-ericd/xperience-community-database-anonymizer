using System.Security.Cryptography;

namespace XperienceCommunity.DatabaseAnonymizer
{
    internal static class SimpleEncryptionHelper
    {
        private static readonly byte[] salt = [10, 20, 30, 40, 50, 60, 70, 80];


        public static string Encrypt(string plainText, byte[] encryptionKeyBytes)
        {
            byte[] iv = new byte[16];
            byte[] array;
            using (var aes = Aes.Create())
            {
                aes.IV = iv;
                aes.Key = encryptionKeyBytes;
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using MemoryStream memoryStream = new();
                using CryptoStream cryptoStream = new(memoryStream, encryptor, CryptoStreamMode.Write);
                using (StreamWriter streamWriter = new(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                array = memoryStream.ToArray();
            }

            return Convert.ToBase64String(array);
        }


        public static string Decrypt(string cipherText, byte[] encryptionKeyBytes)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(cipherText);
            using var aes = Aes.Create();
            aes.IV = iv;
            aes.Key = encryptionKeyBytes;
            var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using MemoryStream memoryStream = new(buffer);
            using CryptoStream cryptoStream = new(memoryStream, decryptor, CryptoStreamMode.Read);
            using StreamReader streamReader = new(cryptoStream);

            return streamReader.ReadToEnd();
        }


        public static byte[] CreateKey(string password, int keyBytes = 32)
        {
            const int Iterations = 300;
            var keyGenerator = new Rfc2898DeriveBytes(password, salt, Iterations);

            return keyGenerator.GetBytes(keyBytes);
        }
    }
}
