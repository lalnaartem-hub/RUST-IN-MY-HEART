using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AdvancedRAT.Utils
{
    public class CryptoHelper
    {
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to your needs
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        public static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to your needs
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        public static string GetHash(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(inputString)))
            {
                sb.Append(b.ToString("X2"));
            }
            return sb.ToString();
        }

        public static byte[] GenerateRandomSalt()
        {
            byte[] data = new byte[0x10];
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(data);
            }
            return data;
        }

        public static string EncryptString(string text, string password)
        {
            byte[] textBytes = Encoding.UTF8.GetBytes(text);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] encryptedBytes = AES_Encrypt(textBytes, passwordBytes);
            return Convert.ToBase64String(encryptedBytes);
        }

        public static string DecryptString(string encryptedText, string password)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] decryptedBytes = AES_Decrypt(encryptedBytes, passwordBytes);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}