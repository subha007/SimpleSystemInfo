using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.WSIException
{
    public interface IExceptionMessage
    {
        /// <summary>
        /// Get or set the message for the class object
        /// </summary>
        Dictionary<ExceptionLevel, List<string>> Message { get; }

        /// <summary>
        /// Add the log 
        /// </summary>
        /// <param name="level">The level of the message logged</param>
        /// <param name="message">The message</param>
        void AddLog(ExceptionLevel level, string message);
    }
}
