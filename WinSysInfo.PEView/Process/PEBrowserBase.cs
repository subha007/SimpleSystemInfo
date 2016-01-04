using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// The link contains PE format parser base https://code.google.com/p/corkami/wiki/PE
    /// </summary>
    public class PEBrowserBase
    {
        /// <summary>
        /// Use of internal Reader wrapper
        /// </summary>
        public BinaryReader Reader { get; set; }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        /// <param name="fullPEFilePath"></param>
        public PEBrowserBase(string fullPEFilePath)
        {
            //this.Reader = new BinaryReader(fullPEFilePath);
        }
    }
}
