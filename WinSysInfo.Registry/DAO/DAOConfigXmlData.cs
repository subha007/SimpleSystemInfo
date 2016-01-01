using SysInfoInventryWinReg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventryWinReg.DAO
{
    /// <summary>
    /// This static class contains the Xml config files and held in memory
    /// </summary>
    public class DAOConfigXmlData
    {
        /// <summary>
        /// Get or set the config xml data in memory
        /// </summary>
        public ModelRegistryKey ConfigXmlRegistry { get; set; }
    }
}
