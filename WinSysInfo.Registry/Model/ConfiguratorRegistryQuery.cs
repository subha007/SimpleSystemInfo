using SysInfoInventryWinReg.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventryWinReg.Model
{
    /// <summary>
    /// The class to facilitate the query of regitry keys by using filters
    /// </summary>
    public class ConfiguratorRegistryQuery
    {
        /// <summary>
        /// Get or set the process 
        /// </summary>
        public EnumRegistryQueryProcess ProcQueryEnum { get; set; }

        /// <summary>
        /// Get or set the Level to which to do deep query if the flag 
        /// <see cref="EnumRegistryQueryProcess.ADD_ALL_SUBKEYS"/> is set to true
        /// </summary>
        public int LevelSubkeyQuery { get; set; }

        /// <summary>
        /// Get or set the main root path of the registry to query.
        /// In case this is null then the path must be specified with the model object
        /// <see cref="ModelRegistryKey"/>
        /// If this is provided then it re-initializes the <see cref="ModelRegistryKey"/>
        /// </summary>
        public ModelRegistryPath RegsitryPath { get; set; }

        /// <summary>
        /// Constructor to initialize the query configurator using enumerator and 
        /// </summary>
        /// <param name="eprocQuery"></param>
        public ConfiguratorRegistryQuery(EnumRegistryQueryProcess eprocQuery, int level)
        {
            this.ProcQueryEnum = eprocQuery;
            this.LevelSubkeyQuery = level;
        }

        /// <summary>
        /// Constructor to initialize the query configurator using enumerator
        /// </summary>
        /// <param name="eprocQuery">The enumerator of registry query process</param>
        public ConfiguratorRegistryQuery(EnumRegistryQueryProcess eprocQuery)
        {
            this.ProcQueryEnum = eprocQuery;
            this.LevelSubkeyQuery = -1;
        }

        /// <summary>
        /// Constructor to initialize the query configurator
        /// </summary>
        public ConfiguratorRegistryQuery() : this(EnumRegistryQueryProcess.NONE, -1) { }

        /// <summary>
        /// Constructor to initilaize the Regsitry path to query
        /// </summary>
        /// <param name="path"></param>
        public ConfiguratorRegistryQuery(ModelRegistryPath path)
        {
            this.RegsitryPath = path;
            this.ProcQueryEnum = EnumRegistryQueryProcess.ADD_ALL_VALUES | EnumRegistryQueryProcess.ADD_ALL_SUBKEYS;
            this.LevelSubkeyQuery = -1;
        }

        /// <summary>
        /// Validate the object on the basis of data
        /// </summary>
        /// <returns></returns>
        public bool Validate(bool throwExcp)
        {
            if(this.RegsitryPath.Validate(throwExcp) == false)
            {
                this.ProcQueryEnum = EnumRegistryQueryProcess.ADD_ALL_VALUES | EnumRegistryQueryProcess.ADD_ALL_SUBKEYS;
                return false;
            }

            return true;
        }
    }
}
