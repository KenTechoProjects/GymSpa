using System;
using System.Security.Cryptography;
using System.Text;

namespace Utilities
{
    public class Cipher
    {
        public static string GenerateHash(string data)
        {
            string hash = string.Empty;
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] dataByteValue = Encoding.UTF8.GetBytes(data);
                var hashedByte = sha512Hash.ComputeHash(dataByteValue);
                hash = BitConverter.ToString(hashedByte).Replace("-", String.Empty);
            }
            return hash.ToLower();
        }

        public static bool VerifyHash(string data, string hash)
        {
            string hashedData = GenerateHash(data);
            return hashedData == hash.ToLower();
        }

        public static string Encrypt(string toEncrypt, string encryptionKey)
        {
            if (string.IsNullOrWhiteSpace(toEncrypt)) return null;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            var IV = UTF8Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");

            keyArray = UTF8Encoding.UTF8.GetBytes(encryptionKey); //encryptionKey = civwydt7vy87hsd2

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = keyArray;
            //aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.IV = IV;
            aes.Padding = PaddingMode.PKCS7;

            aes.Mode = CipherMode.CBC;
            var transform = aes.CreateEncryptor();
            var outputArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            aes.Clear();
            return Convert.ToBase64String(outputArray, 0, outputArray.Length);
        }

        public static string Decrypt(string toDecrypt, string decryptionKey)
        {
            if (string.IsNullOrWhiteSpace(toDecrypt)) return null;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);
            byte[] IV = UTF8Encoding.UTF8.GetBytes("HR$2pIjHR$2pIj12");

            byte[] keyArray = UTF8Encoding.UTF8.GetBytes(decryptionKey);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.Key = keyArray;
            //aes.KeySize = 256;
            aes.BlockSize = 128;
            aes.IV = IV;
            aes.Padding = PaddingMode.PKCS7;

            aes.Mode = CipherMode.CBC;
            var transform = aes.CreateDecryptor();
            var outputArray = transform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            aes.Clear();
            return UTF8Encoding.UTF8.GetString(outputArray);
        }
    }
}