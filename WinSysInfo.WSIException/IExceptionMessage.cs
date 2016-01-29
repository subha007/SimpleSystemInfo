using System.Collections.Generic;

namespace WinSysInfo.WSIException
{
    public interface IExceptionMessage
    {
        /// <summary>
        /// Get or set the message for the class object
        /// </summary>
        List<ExceptionModel> MessageList { get; }

        /// <summary>
        /// Add the log 
        /// </summary>
        /// <param name="level">The level of the message logged</param>
        /// <param name="message">The message</param>
        void AddLog(ExceptionModel model);
    }
}
