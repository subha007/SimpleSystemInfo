using log4net;
using SysInfoInventryWinReg.Generic;
using System;
using System.IO;

namespace SysInfoInventryWinReg.Model
{
    /// <summary>
    /// The class is used as a filter  during the processing of xml config file data using
    /// </summary>
    public class ConfiguratorBaseConfig
    {
        /// <summary>
        /// Get the log instance for this class to create log.
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ConfiguratorBaseConfig));

        /// <summary>
        /// Get or set if to use configurator or skip during the processing
        /// </summary>
        public bool UseConfigurator { get; set; }

        /// <summary>
        /// As cutomised set and get is not possible with auto properties
        /// </summary>
        protected string configPath;
        /// <summary>
        /// Get or set the relative path of the xml file to read
        /// </summary>
        public string ConfigPath 
        {
            get { return configPath; }
            set { configPath = NormalizePath(value); }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="useConfig">If to use the filter config or skip.</param>
        /// <param name="relPath">The relative path of config file. Relative to the current environment path or the application path.</param>
        public ConfiguratorBaseConfig(bool useConfig, string relPath)
        {
            UseConfigurator = useConfig;
            ConfigPath = NormalizePath(relPath);
        }

        /// <summary>
        /// Check and rationalize the path string to full path.
        /// If the path is relative then prepend the application path if not provided
        /// with root path.
        /// </summary>
        /// <param name="relPath"></param>
        /// <returns></returns>
        private string NormalizePath(string relPath)
        {
            if (System.IO.Path.IsPathRooted(relPath) == false)
                return Path.Combine(ConstantsConfig.ApplicationPath, relPath);

            return relPath;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="useConfig">If to use the filter config or skip.</param>
        public ConfiguratorBaseConfig(bool useConfig) : this(useConfig, string.Empty) { }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="relPath">The relative path of config file. Relative to the current environment path or the application path.</param>
        public ConfiguratorBaseConfig(string relPath) : this(true, relPath) { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfiguratorBaseConfig() : this(true, string.Empty) { }

        /// <summary>
        /// Manipulated property to check if the config file is to be read as a normal file or as partial
        /// </summary>
        public virtual bool IsConfigReadAsNormalFile
        {
            get
            {
                if (string.IsNullOrEmpty(this.ConfigPath) == true)
                {
                    logger.Fatal("No config file provided to read");
                    throw new ArgumentNullException("RelativePath");
                }

                System.IO.FileInfo fiInfo = new System.IO.FileInfo(this.ConfigPath);
                return ((fiInfo.Length / 1024.0) <= ConstantsConfig.LimitXmlFileReadKB);
            }
        }

        /// <summary>
        /// Manipulated property to check if the config file is to be read as a partial
        /// </summary>
        public virtual bool IsConfigReadAsPartialFile
        {
            get
            {
                return false;
            }
        }
    }
}
