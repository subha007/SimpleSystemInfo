using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Factory;
using WinSysInfo.PEView.Helper;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// The root class used to browse a file in the file browser tree
    /// </summary>
    public class FileBrowser
    {
        /// <summary>
        /// All the browser properties for browsing
        /// </summary>
        public FileBrowserProperty Property { get; set; }
    }
}
