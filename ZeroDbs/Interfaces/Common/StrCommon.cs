using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces.Common
{
    public class StrCommon: IStrCommon
    {
        public string MD5_16(string str)
        {
            return Tools.MD5.To16(str);
        }
        public string MD5_16(byte[] bytes)
        {
            return Tools.MD5.To16(bytes);
        }
        public string MD5_32(string str)
        {
            return Tools.MD5.To32(str);
        }
        public string MD5_32(byte[] bytes)
        {
            return Tools.MD5.To32(bytes);
        }

        public string SHA1(string str)
        {
            return Tools.SHA.SHA1String(str);
        }
        public string SHA1(byte[] bytes)
        {
            return Tools.SHA.SHA1String(bytes);
        }
        public string SHA256(string str)
        {
            return Tools.SHA.SHA256String(str);
        }
        public string SHA256(byte[] bytes)
        {
            return Tools.SHA.SHA256String(bytes);
        }
        public string SHA384(string str)
        {
            return Tools.SHA.SHA384String(str);
        }
        public string SHA384(byte[] bytes)
        {
            return Tools.SHA.SHA384String(bytes);
        }
        public string SHA512(string str)
        {
            return Tools.SHA.SHA512String(str);
        }
        public string SHA512(byte[] bytes)
        {
            return Tools.SHA.SHA512String(bytes);
        }
        public string DESDecrypt(string str)
        {
            return Tools.DES.Decrypt(str);
        }
        public string DESDecrypt(string str, string key)
        {
            return Tools.DES.Decrypt(str, key);
        }
        public string DESDecrypt(string str, string key, string iv)
        {
            return Tools.DES.Decrypt(str, key, iv);
        }
        public string DESEncrypt(string str)
        {
            return Tools.DES.Encrypt(str);
        }
        public string DESEncrypt(string str, string key)
        {
            return Tools.DES.Encrypt(str, key);
        }
        public string DESEncrypt(string str, string key, string iv)
        {
            return Tools.DES.Encrypt(str, key, iv);
        }


    }
}
