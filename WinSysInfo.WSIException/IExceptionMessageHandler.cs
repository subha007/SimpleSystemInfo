namespace WinSysInfo.WSIException
{
    /// <summary>
    /// The handler class for every 
    /// </summary>
    public interface IExceptionMessageHandler
    {
        /// <summary>
        /// Add the log 
        /// </summary>
        /// <param name="level">The level of the message logged</param>
        /// <param name="message">The message</param>
        void AddLog(ExceptionLevel level, string message);
    }
}
