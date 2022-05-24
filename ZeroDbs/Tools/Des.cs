using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Tools
{
    /// <summary>
    /// DES加密/解密类。
    /// </summary>
    public static class DES
    {
        private class KeyIvByteInfo
        {
            public byte[] Key { get; }
            public byte[] IV { get; }
            public KeyIvByteInfo(byte[] key, byte[] iv)
            {
                this.Key = key;
                this.IV = iv;
            }
        }
        private static Dictionary<string, KeyIvByteInfo> _keyChace = new Dictionary<string, KeyIvByteInfo>(12);
        private static object _lock = new object();
        /// <summary>
        /// 默认key值
        /// </summary>
        private static string DEFAULTKEY = "ZERODESKEY";
        private static string DEFAULTIV = "ZERODESIV";

        private static KeyIvByteInfo GetKeyIvCache(string key, string iv)
        {
            if (string.IsNullOrEmpty(key)) { key = "_"; }
            if (string.IsNullOrEmpty(iv)) { iv = "_"; }
            string k = string.Format("{0}_{1}", key, iv);
            KeyIvByteInfo reval;
            if (_keyChace.TryGetValue(key, out reval))
            {
                return reval;
            }
            byte[] keyBytes = ASCIIEncoding.UTF8.GetBytes(MD5.To32(key).Substring(0, 24));
            byte[] ivBytes = ASCIIEncoding.UTF8.GetBytes(MD5.To32(iv).Substring(0, 8));
            reval = new KeyIvByteInfo(keyBytes, ivBytes);
            lock (_lock)
            {
                if (_keyChace.ContainsKey(key))
                {
                    _keyChace[key] = reval;
                }
                else
                {
                    _keyChace.Add(key, reval);
                }
            }
            return reval;
        }

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
        public static string Encrypt(string Text, string sKey)
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
            var t = GetKeyIvCache(sKey, sIV);
            des.Key = t.Key;
            des.IV = t.IV;
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
            var t = GetKeyIvCache(sKey, sIV);
            des.Key = t.Key;
            des.IV = t.IV;
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
