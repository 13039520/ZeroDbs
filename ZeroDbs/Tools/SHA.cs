using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroDbs.Tools
{
    public static class SHA
    {
        public static byte[] SHA1(byte[] buffer)
        {
            var sha = System.Security.Cryptography.SHA1.Create();
            var temp = sha.ComputeHash(buffer);
            sha.Dispose();
            return temp;
        }
        public static byte[] SHA256(byte[] buffer)
        {
            var sha = System.Security.Cryptography.SHA256.Create();
            var temp = sha.ComputeHash(buffer);
            sha.Dispose();
            return temp;
        }
        public static byte[] SHA384(byte[] buffer)
        {
            var sha = System.Security.Cryptography.SHA384.Create();
            var temp = sha.ComputeHash(buffer);
            sha.Dispose();
            return temp;
        }
        public static byte[] SHA512(byte[] buffer)
        {
            var sha = System.Security.Cryptography.SHA512.Create();
            var temp = sha.ComputeHash(buffer);
            sha.Dispose();
            return temp;
        }


        public static byte[] SHA1(string str)
        {
            return SHA1(System.Text.Encoding.UTF8.GetBytes(str));
        }
        public static string SHA1String(string str)
        {
            return Convert.ToBase64String(SHA1(str));
        }
        public static string SHA1String(byte[] buffer)
        {
            return Convert.ToBase64String(SHA1(buffer));
        }

        public static byte[] SHA256(string str)
        {
            return SHA256(System.Text.Encoding.UTF8.GetBytes(str));
        }
        public static string SHA256String(string str)
        {
            return Convert.ToBase64String(SHA256(str));
        }
        public static string SHA256String(byte[] buffer)
        {
            return Convert.ToBase64String(SHA256(buffer));
        }

        public static byte[] SHA384(string str)
        {
            return SHA384(System.Text.Encoding.UTF8.GetBytes(str));
        }
        public static string SHA384String(string str)
        {
            return Convert.ToBase64String(SHA384(str));
        }
        public static string SHA384String(byte[] buffer)
        {
            return Convert.ToBase64String(SHA384(buffer));
        }

        public static byte[] SHA512(string str)
        {
            return SHA512(System.Text.Encoding.UTF8.GetBytes(str));
        }
        public static string SHA512String(string str)
        {
            return Convert.ToBase64String(SHA512(str));
        }
        public static string SHA512String(byte[] buffer)
        {
            return Convert.ToBase64String(SHA512(buffer));
        }

    }
}
