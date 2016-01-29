using System;

namespace SysInfoInventryWinReg.Generic
{
    /// <summary>
    /// All the entries in config file
    /// </summary>
    public static class ConstantsConfig
    {
        /// <summary>
        /// Get the application path
        /// </summary>
        public static readonly string ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        /// Get the limit of file size from app config which chnages the way XMl serialization happens
        /// </summary>
        public static readonly double LimitXmlFileReadKB = Convert.ToDouble(System.Configuration.ConfigurationManager.AppSettings["xmlFileSizeReadLimitKB"]);
    }
}
