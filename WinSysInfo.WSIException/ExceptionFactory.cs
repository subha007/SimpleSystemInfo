using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.WSIException
{
    /// <summary>
    /// Get the genral exception object
    /// </summary>
    public class ExceptionFactory
    {
        /// <summary>
        /// Get a new instance of exception class
        /// </summary>
        /// <returns></returns>
        public static IExceptionMessage GetNewInstance()
        {
            return new CustomExceptionMessage();
        }
    }
}
