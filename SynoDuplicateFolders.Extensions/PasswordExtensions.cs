
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using static SynoDuplicateFolders.Extensions.DpApiString;

namespace SynoDuplicateFolders.Extensions
{
    public class WrappedPassword<T>
        where T : class
    {
        private readonly PropertyInfo _property;
        private readonly T _instance;
        private static byte[] vector = null;
        private static readonly object vectorlock = new object();
        private static DataProtectionScope scope = DataProtectionScope.CurrentUser;
        public WrappedPassword(string propname, T instance)
        {
            lock (vectorlock)
            {
                if (vector == null)
                {
                    vector = Encoding.Unicode.GetBytes("Is a gift a gift without wrapping?");
                }
            }
            _instance = instance;
            _property = _instance.GetType().GetProperty(propname);

        }
        public T Value { get { return _instance; } }
        public string Password
        {
            get
            {
                return ToInsecureString(DecryptString((string)_property.GetValue(_instance), scope, vector));
            }
            set
            {
                _property.SetValue(_instance, EncryptString(ToSecureString(value),scope,vector));
            }
        }
        public static void SetEntropy(string data, DataProtectionScope scope = DataProtectionScope.CurrentUser)
        {
            lock (vectorlock)
            {
                vector = Encoding.Unicode.GetBytes(data);
            }
        }
    }


    public static class DpApiString
    {
            
        public static string EncryptString(SecureString input, DataProtectionScope scope, byte[] entropy)
        {
            return Convert.ToBase64String(ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)), entropy, scope));
        }

        public static SecureString DecryptString(string encryptedData, DataProtectionScope scope, byte[] entropy)
        {
            try
            {
                return ToSecureString(Encoding.Unicode.GetString(ProtectedData.Unprotect(Convert.FromBase64String(encryptedData), entropy, scope)));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString result = new SecureString();
            foreach (char c in input)
            {
                result.AppendChar(c);
            }
            result.MakeReadOnly();
            return result;
        }

        public static string ToInsecureString(SecureString input)
        {
            string result = string.Empty;
            IntPtr ptr = Marshal.SecureStringToBSTR(input);
            try
            {
                result = Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                Marshal.ZeroFreeBSTR(ptr);
            }
            return result;
        }
    }

}

