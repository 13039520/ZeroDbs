using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Tools
{
    public static class MD5
    {
        public static byte[] Hash32(string str)
        {
            return Hash32(System.Text.Encoding.UTF8.GetBytes(str));
        }
        public static byte[] Hash32(byte[] buffer)
        {
            System.Security.Cryptography.MD5 mD5 = System.Security.Cryptography.MD5.Create();
            byte[] result = mD5.ComputeHash(buffer);
            mD5.Dispose();
            return result;
        }
        public static byte[] Hash16(string str)
        {
            return Hash16(System.Text.Encoding.UTF8.GetBytes(str));
        }
        public static byte[] Hash16(byte[] buffer)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider mD5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] result = mD5.ComputeHash(buffer);
            mD5.Dispose();
            return result;
        }
        public static string To32(string str)
        {
            byte[] data = Hash32(str);
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            return s.ToString();
        }
        public static string To32(byte[] buffer)
        {
            byte[] data = Hash32(buffer);
            StringBuilder s = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }
            return s.ToString();
        }
        public static string To16(string str)
        {
            byte[] data = Hash16(str);
            string s = BitConverter.ToString(data, 4, 8);
            return s.Replace("-", "");
        }
        public static string To16(byte[] buffer)
        {
            byte[] data = Hash16(buffer);
            string s = BitConverter.ToString(data, 4, 8);
            return s.Replace("-", "");
        }
    }
}
