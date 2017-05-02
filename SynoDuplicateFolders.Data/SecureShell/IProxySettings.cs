using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SynoDuplicateFolders.Data.SecureShell
{
    public interface IProxySettings 
    {
        string ProxyType { get; }
        string Host { get; }
        int Port { get; }        
        string UserName { get; }
        string Password { get; }
    }
}
