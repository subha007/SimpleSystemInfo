using log4net;
using Microsoft.Win32;
using SysInfoInventryWinReg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.WSIException;

namespace SysInfoInventryWinReg.Process
{
    /// <summary>
    /// A class which queries the Windows registry to find system information.
    /// It uses Win32 <see cref="Registry"/> class and <see cref="RegistryKey"/> class.
    /// This class uses the Xml config file to read the config keys for which registry needs to be read.
    /// This class contains and assumes most basic way of reading registry. If you need it to be using
    /// custom values then override the function in a derived class.
    /// </summary>
    public class ProcessRegistryQuery
    {
        /// <summary>
        /// Get the log instance for this class to create log.
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ProcessRegistryQuery));

        /// <summary>
        /// Get the model of config file used to read registry settings
        /// Generally the model is created using <see cref="ProcessRegistryConfigData"/>
        /// </summary>
        public ModelRegistryKey RegKey { get; set; }

        /// <summary>
        /// Get or set the type of filtering
        /// </summary>
        public ConfiguratorRegistryQuery QueryFilter { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessRegistryQuery() 
            : this(null, new ConfiguratorRegistryQuery()) { }

        /// <summary>
        /// Constructor when the regsitry path is provided
        /// </summary>
        /// <param name="regPath"></param>
        public ProcessRegistryQuery(ConfiguratorRegistryQuery configurator) : this(null, configurator) { }

        /// <summary>
        /// Using the actual model object created in memory from the config file and the type of process.
        /// </summary>
        /// <param name="regKeyModel">The model object reference</param>
        /// <param name="enumProcQuery">The process query type. Default is NONE.</param>
        public ProcessRegistryQuery(ModelRegistryKey regKeyModel, ConfiguratorRegistryQuery configurator)
        {
            this.RegKey = regKeyModel;
            this.QueryFilter = configurator;
        }

        /// <summary>
        /// The basic way of reading a registry key and its Values.
        /// It do not read any subkeys. It only reads the key and the values.
        /// Override this method in derived class to make it specific.
        /// </summary>
        public virtual void Query()
        {
            // Validate the data and throw exception
            this.Validate(true);

            // Read the registry data as mentioned in the config entry
            ReadRegistry(this.RegKey);
        }

        /// <summary>
        /// Read the windows registry for every path entry in the config file and fill the Values.
        /// </summary>
        protected void ReadRegistry(ModelRegistryKey regKeyModel)
        {
            // Read the registry
            ReadRegistryValues(regKeyModel);

            if (this.QueryFilter.ProcQueryEnum.DoAddAllSubKeys() == true ||
                regKeyModel.SubKeys != null ||
                ((this.QueryFilter.LevelSubkeyQuery > regKeyModel.TreeLevel) || this.QueryFilter.LevelSubkeyQuery == -1))
            {
                ReadRegistrySubKeys(regKeyModel);
            }
        }

        /// <summary>
        /// Read the registry sub keys
        /// </summary>
        /// <param name="modelRegistryKey"></param>
        private void ReadRegistrySubKeys(ModelRegistryKey regKeyModel)
        {
            // Get the registryKey instance
            using(RegistryKey rootRegKey = InitializeRegistryKey(regKeyModel.RegsitryPath.RootPath, regKeyModel.RegsitryPath.SubKeyPath, RegistryView.Registry32))
            {
                string[] subkeyNames = rootRegKey.GetSubKeyNames();
                if (subkeyNames.Length <= 0)
                    return;

                if (this.QueryFilter.ProcQueryEnum.DoAddAllSubKeys() == true)
                {
                    if (regKeyModel.SubKeys == null)
                        regKeyModel.SubKeys = new List<ModelRegistryKey>();
                    foreach (string subKeyName in subkeyNames)
                    {
                        ModelRegistryKey subKey = regKeyModel.GetSubKeyObject(subKeyName);
                        ReadRegistry(subKey);
                    }
                }
                else
                {
                    foreach (ModelRegistryKey keyValue in regKeyModel.SubKeys)
                    {
                        ReadRegistry(keyValue);
                    }
                }
            }
        }

        /// <summary>
        /// Read the windows registry for only one path entry
        /// </summary>
        /// <param name="regPathQuery"></param>
        protected void ReadRegistryValues(ModelRegistryKey regPathQuery)
        {
            // Get the registryKey instance
            using (RegistryKey rootRegKey = InitializeRegistryKey(regPathQuery.RegsitryPath.RootPath, regPathQuery.RegsitryPath.SubKeyPath, RegistryView.Registry32))
            {
                // Add only those values requested for
                if (this.QueryFilter.ProcQueryEnum.DoAddAllValues() == false)
                {
                    foreach (ModelRegistryKeyValue keyValue in regPathQuery.KeyValuePairs)
                    {
                        ReadRegistryValue(rootRegKey, keyValue);
                    }
                }
                else
                {
                    // Add all the values
                    foreach(string valueName in rootRegKey.GetValueNames())
                    {
                        ModelRegistryKeyValue keyValue = regPathQuery.GetValueObject(valueName);
                        ReadRegistryValue(rootRegKey, keyValue);
                    }
                }
            }
        }

        /// <summary>
        /// Internal method to get and save the registry value entry
        /// </summary>
        /// <param name="rootRegKey"></param>
        /// <param name="keyValue"></param>
        private void ReadRegistryValue(RegistryKey rootRegKey, ModelRegistryKeyValue keyValue)
        {
            object objValue = rootRegKey.GetValue(keyValue.Name);
            if (objValue == null)
            {
                logger.Warn(keyValue.Name + " not found in registry");
                keyValue.AddLog(ExceptionLevel.WARN, keyValue.Name + " not found in registry");
            }
            else
                keyValue.ParseAndSetValue(objValue, rootRegKey.GetValueKind(keyValue.Name));
        }

        /// <summary>
        /// Initialize the registry key instance depndening on the root level name and sub key path
        /// </summary>
        /// <param name="rootPath">The string value of the root key</param>
        /// <param name="subKeyPath">The subkey path.</param>
        /// <returns></returns>
        private RegistryKey InitializeRegistryKey(RegistryHive rootPath, string subKeyPath, RegistryView regViewType)
        {
            return RegistryKey.OpenBaseKey(rootPath, regViewType);
        }

        /// <summary>
        /// Validate the object on the basis of data
        /// </summary>
        /// <returns></returns>
        protected bool Validate(bool throwExcp)
        {
            if (this.RegKey == null)
            {
                if (this.QueryFilter == null)
                    throw new Exception("Not implemented");

                this.QueryFilter.Validate(true);

                this.RegKey = new ModelRegistryKey(this.QueryFilter.RegsitryPath);
            }
            
            return true;
        }
    }
}
