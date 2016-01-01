using log4net;
using SysInfoInventryWinReg.Generic;
using SysInfoInventryWinReg.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventryWinReg.Process
{
    /// <summary>
    /// This class is based on reading the Registry key whose address is 
    /// HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\Uninstall and can find many subkeys which helps
    /// to have different details of installed applications
    /// </summary>
    public class ProcessInstalledProductsRegistryQuery : ProcessRegistryQuery
    {
        /// <summary>
        /// Get the log instance for this class to create log.
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ProcessInstalledProductsRegistryQuery));

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessInstalledProductsRegistryQuery()
            : base(null, new ConfiguratorRegistryQuery(ConstantRegistryPath.UninstalledRegistryPath))
        { }
    }
}
