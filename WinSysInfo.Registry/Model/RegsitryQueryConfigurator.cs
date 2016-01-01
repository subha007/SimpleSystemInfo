using SysInfoInventry.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventry.Model
{
    /// <summary>
    /// The class to facilitate the query of regitry keys by using filters
    /// </summary>
    public class RegsitryQueryConfigurator
    {
        /// <summary>
        /// Get or set the process 
        /// </summary>
        public EnumRegistryQueryProcess ProcQueryEnum { get; set; }
    }
}
