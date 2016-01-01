using SysInfoInventryWinReg.Generic;
using SysInfoInventryWinReg.Model;
using SysInfoInventryWinReg.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventryWinReg.Manager
{
    /// <summary>
    /// The class which manages the query of processing windows registry data.
    /// It controls the Access of Registry data by external applications and processes the query
    /// on registry and sends back the data model.
    /// </summary>
    public class ManagerRegistryQuery
    {
        /// <summary>
        /// Get or set the data model instance
        /// </summary>
        public ModelRegistryKey RegModel
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or set the regsitry config file configurator
        /// </summary>
        public ConfiguratorRegistryConfig ConfiguratorOfRegistryConfigReader
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or set the configurator for the query process
        /// </summary>
        public ConfiguratorRegistryQuery ConfiguratorofRegistryQuery
        {
            get;
            private set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ManagerRegistryQuery()
        {
            this.ConfiguratorOfRegistryConfigReader = new ConfiguratorRegistryConfig();
            this.ConfiguratorofRegistryQuery = new ConfiguratorRegistryQuery();
        }

        /// <summary>
        /// The default constructor to initialize the manager instance of querying the Windows regsitry
        /// </summary>
        /// <param name="ePathName"></param>
        /// <param name="queryprocConfigurator"></param>
        public ManagerRegistryQuery(ConfiguratorRegistryConfig regConfigReaderConfigurator, ConfiguratorRegistryQuery queryprocConfigurator)
        {
            this.ConfiguratorOfRegistryConfigReader = regConfigReaderConfigurator;
            this.ConfiguratorofRegistryQuery = queryprocConfigurator;
        }

        /// <summary>
        /// The main method to populate the registry model data
        /// </summary>
        public void ReadAllRegsitry()
        {
            // Load the full Xml Config file
            // Uses the read config file process
            //ProcessConfigData configDataReaderprocess = new ProcessConfigData(this.ConfiguratorOfRegistryConfigReader);
            //configDataReaderprocess.ReadConfig();
        }
    }
}
