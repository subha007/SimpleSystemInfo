using System;
using System.Collections.Generic;

namespace WinSysInfo.WSIException
{
    /// <summary>
    /// This interface is used by any class which wants to hold the error/warning messages seperately for
    /// each object.
    /// </summary>
    public class CustomExceptionMessage : Exception, IExceptionMessage
    {
        /// <summary>
        /// Get or set the message for the class object
        /// </summary>
        public List<ExceptionModel> MessageList 
        {
            get;
            protected set; 
        }

        /// <summary>
        /// Add the log 
        /// </summary>
        /// <param name="level">The level of the message logged</param>
        /// <param name="message">The message</param>
        public void AddLog(ExceptionModel model)
        {
            if (this.MessageList == null)
                this.MessageList = new List<ExceptionModel>();

            this.MessageList.Add(model);
        }

        private void Initialize()
        {
            this.MessageList = new List<ExceptionModel>();
        }

        /// <summary>
        /// Basic construction
        /// </summary>
        public CustomExceptionMessage() : this(new ExceptionModel()) { }

        /// <summary>
        /// Set Message
        /// </summary>
        /// <param name="message"></param>
        public CustomExceptionMessage(ExceptionModel eModel)
        {
            this.Initialize();
            this.AddLog(eModel);
        }
    }
}
