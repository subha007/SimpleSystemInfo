using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Interface
{
    /// <summary>
    /// A generic interface to browse through the PE / COFF file
    /// </summary>
    public interface ICOFFBrowser
    {
        /// <summary>
        /// Get or set the reader strategy
        /// </summary>
        IFileReadStrategy ReaderStrategy { get; set; }

        /// <summary>
        /// Data mapping
        /// </summary>
        ICOFFNavigator Navigator { get; set; }
    }
}
