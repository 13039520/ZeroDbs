using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Interfaces
{
    public interface IStrCommon
    {
        string MD5_16(string str);
        string MD5_16(byte[] bytes);
        string MD5_32(string str);
        string MD5_32(byte[] bytes);

        string SHA1(string str);
        string SHA1(byte[] bytes);
        string SHA256(string str);
        string SHA256(byte[] bytes);
        string SHA384(string str);
        string SHA384(byte[] bytes);
        string SHA512(string str);
        string SHA512(byte[] bytes);

        string DESDecrypt(string str);
        string DESDecrypt(string str, string key);
        string DESDecrypt(string str, string key, string iv);
        string DESEncrypt(string str);
        string DESEncrypt(string str, string key);
        string DESEncrypt(string str, string key, string iv);

    }
}
