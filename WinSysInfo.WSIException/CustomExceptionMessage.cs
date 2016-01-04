using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.WSIException
{
    /// <summary>
    /// This interface is used by any class which wants to hold the error/warning messages seperately for
    /// each object.
    /// </summary>
    public class CustomExceptionMessage : IExceptionMessage
    {
        /// <summary>
        /// Get or set the message for the class object
        /// </summary>
        public Dictionary<ExceptionLevel, List<string>> Message 
        {
            get;
            protected set; 
        }

        /// <summary>
        /// Add the log 
        /// </summary>
        /// <param name="level">The level of the message logged</param>
        /// <param name="message">The message</param>
        public void AddLog(ExceptionLevel level, string message)
        {
            if (this.Message == null)
                this.Message = new Dictionary<ExceptionLevel, List<string>>();

            if (this.Message.ContainsKey(level) == false)
            {
                this.Message.Add(level, new List<string>(new string[] { message }));
            }
            else
            {
                this.Message[level].Add(message);
            }
        }
    }
}
