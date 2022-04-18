using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace ZeroDbs.Tools
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public static class DES
    {
        /// <summary>
        /// 默认key值
        /// </summary>
        private static string DEFAULTKEY = "ZERODESKEY";
        private static string DEFAULTIV = "ZERODESIV";

        #region ========加密========

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Encrypt(string Text)
        {
            return Encrypt(Text, DEFAULTKEY);
        }
        public static string Encrypt(string Text,string sKey)
        {
            return Encrypt(Text, sKey, DEFAULTIV);
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <param name="sIV"></param>
        /// <returns></returns> 
        public static string Encrypt(string Text, string sKey, string sIV)
        {
            System.Security.Cryptography.TripleDES des = System.Security.Cryptography.TripleDES.Create();
            des.Padding = PaddingMode.PKCS7;
            byte[] inputByteArray;
            inputByteArray = Encoding.UTF8.GetBytes(Text);
            string key = MD5.To16(sKey).Substring(0, 8);
            des.Key = ASCIIEncoding.UTF8.GetBytes(key);
            string iv= MD5.To16(sIV).Substring(0, 8);
            des.IV = ASCIIEncoding.UTF8.GetBytes(iv);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string reval = Convert.ToBase64String(ms.ToArray());
            ms.Close();
            cs.Close();
            return reval;
        }

        #endregion

        #region ========解密========

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="Text"></param>
        /// <returns></returns>
        public static string Decrypt(string Text)
        {
            return Decrypt(Text, DEFAULTKEY);
        }
        public static string Decrypt(string Text, string sKey)
        {
            return Decrypt(Text, sKey, DEFAULTIV);
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string sKey, string sIV)
        {
            System.Security.Cryptography.TripleDES des = System.Security.Cryptography.TripleDES.Create();
            des.Padding = PaddingMode.PKCS7;
            byte[] inputByteArray = Convert.FromBase64String(Text);
            string key = MD5.To16(sKey).Substring(0, 8);
            des.Key = ASCIIEncoding.UTF8.GetBytes(key);
            string iv = MD5.To16(sIV).Substring(0, 8);
            des.IV = ASCIIEncoding.UTF8.GetBytes(iv);
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);
            
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string reval = Encoding.UTF8.GetString(ms.ToArray());
            return reval;
        }

        #endregion

    }
}
