using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Amo.Lib
{
    /// <summary>
    /// 字符加密
    /// </summary>
    public class Encryption
    {
        /// <summary>
        /// 利用默认的密钥加密一个字符串
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <returns>密文</returns>
        public static string Encrypt(string pToEncrypt)
        {
            return Encrypt(pToEncrypt, "4Jka9N3y"); // 为密钥
        }

        /// <summary>
        /// 利用DES加密一个字符串
        /// </summary>
        /// <param name="pToEncrypt">要加密的字符串</param>
        /// <param name="sKey">密钥</param>
        /// <returns>密文</returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // 将字符串转化为一个byte数组
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);

            // Create the crypto objects, with the key, as passed in
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the byte array into the crypto stream
            // (It will end up in the memory stream)
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            // Get the data back from the memory stream, and into a string
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                // Format as hex
                ret.AppendFormat("{0:X2}", b);
            }

            return ret.ToString();
        }

        /// <summary>
        /// 用默认的密钥解密
        /// </summary>
        /// <param name="pToDecrypt">待解密的密文</param>
        /// <returns>明文</returns>
        public static string Decrypt(string pToDecrypt)
        {
            return Decrypt(pToDecrypt, "4Jka9N3y");
        }

        /// <summary>
        /// 解密一个字符串
        /// </summary>
        /// <param name="pToDecrypt">密文</param>
        /// <param name="sKey">密钥</param>
        /// <returns>明文</returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            if (string.IsNullOrEmpty(pToDecrypt) || string.IsNullOrEmpty(sKey))
            {
                return string.Empty;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            // Put the input string into the byte array
            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16);
                inputByteArray[x] = (byte)i;
            }

            // Create the crypto objects
            des.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            des.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            // Flush the data through the crypto stream into the memory stream
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            // Get the decrypted data back from the memory stream
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.Append((char)b);
            }

            return ret.ToString();
        }

        /// <summary>
        /// SHA1加密
        /// </summary>
        /// <param name="strIN">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA1(string strIN)
        {
            SHA1 sha1 = System.Security.Cryptography.SHA1.Create();
            byte[] tmpByte = sha1.ComputeHash(GetKeyByteArray(strIN));
            string rst = BitConverter.ToString(tmpByte);
            sha1.Dispose();
            return rst;
        }

        /// <summary>
        /// SHA256加密
        /// </summary>
        /// <param name="strIN">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string SHA256(string strIN)
        {
            SHA256 sha256 = new SHA256Managed();
            byte[] tmpByte = sha256.ComputeHash(GetKeyByteArray(strIN));

            StringBuilder rst = new StringBuilder();
            for (int i = 0; i < tmpByte.Length; i++)
            {
                rst.Append(tmpByte[i].ToString("x2"));
            }

            sha256.Clear();
            return rst.ToString();
        }

        /// <summary>
        /// 转成Base64
        /// </summary>
        /// <param name="message">输入字符串</param>
        /// <returns>Base64编码</returns>
        public static string Base64(string message)
        {
            System.Text.Encoding encode = System.Text.Encoding.UTF8;
            byte[] bytedata = encode.GetBytes(message);
            string strPath = Convert.ToBase64String(bytedata, 0, bytedata.Length);
            return strPath;
        }

        /// <summary>
        /// HmacSHA256加密
        /// </summary>
        /// <param name="message">要加密的字符串</param>
        /// <param name="secret">key</param>
        /// <returns>加密后的字符串</returns>
        public static string HmacSHA256(string message, string secret)
        {
            secret = secret ?? string.Empty;
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="str">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = string.Empty;
            for (int i = 0; i < b.Length; i++)
            {
                ret += b[i].ToString("x").PadLeft(2, '0');
            }

            return ret;
        }

        /// <summary>
        /// HmacMD5加密
        /// </summary>
        /// <param name="message">要加密的字符串</param>
        /// <param name="secret">Key</param>
        /// <returns>加密后的字符串</returns>
        public static string HmacMD5(string message, string secret)
        {
            secret = secret ?? string.Empty;
            byte[] keyByte = GetKeyByteArray(secret);
            byte[] messageBytes = GetKeyByteArray(message);
            StringBuilder rsb = new StringBuilder();
            using (var hmacmd5 = new HMACMD5(keyByte))
            {
                byte[] hashmessage = hmacmd5.ComputeHash(messageBytes);

                for (int i = 0; i < hashmessage.Length; i++)
                {
                    rsb.Append(hashmessage[i].ToString("x2"));
                }
            }

            return rsb.ToString();
        }

        private static byte[] GetKeyByteArray(string strKey)
        {
            UTF8Encoding asc = new UTF8Encoding();
            int tmpStrLen = strKey.Length;
            byte[] tmpByte = new byte[tmpStrLen - 1];
            tmpByte = asc.GetBytes(strKey);
            return tmpByte;
        }
    }
}
