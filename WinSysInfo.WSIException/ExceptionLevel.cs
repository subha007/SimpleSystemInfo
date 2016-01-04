using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.WSIException
{
    /// <summary>
    /// This enumerator class describes the exception level of logging
    /// </summary>
    public enum ExceptionLevel
    {
        NONE = 0,
        DEBUG = 1,
        INFO = 2,
        WARN = 3,
        ERROR = 4
    }
}
