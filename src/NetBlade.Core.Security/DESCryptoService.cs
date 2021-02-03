using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NetBlade.Core.Security
{
    public static class DESCryptoService
    {
        private static readonly byte[] IV = new byte[8] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static string Desencrypt(string key, string value)
        {
            byte[] bytekey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            byte[] bytevalue = Convert.FromBase64String(value);
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider sagCrypt = new DESCryptoServiceProvider
            {
                Key = bytekey,
                IV = DESCryptoService.IV
            };

            ICryptoTransform desencrypt = sagCrypt.CreateDecryptor();
            CryptoStream cryptstream = new CryptoStream(ms, desencrypt, CryptoStreamMode.Write);

            cryptstream.Write(bytevalue, 0, bytevalue.Length);
            cryptstream.FlushFinalBlock();

            return Encoding.UTF8.GetString(ms.ToArray());
        }

        public static string Encrypt(string key, string value)
        {
            byte[] bytearray = Encoding.UTF8.GetBytes(value);
            byte[] bytekey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
            MemoryStream ms = new MemoryStream();
            DESCryptoServiceProvider sagCrypt = new DESCryptoServiceProvider { Key = bytekey, IV = DESCryptoService.IV };
            ICryptoTransform desencrypt = sagCrypt.CreateEncryptor();
            CryptoStream cryptstream = new CryptoStream(ms, desencrypt, CryptoStreamMode.Write);
            cryptstream.Write(bytearray, 0, bytearray.Length);
            cryptstream.FlushFinalBlock();

            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
