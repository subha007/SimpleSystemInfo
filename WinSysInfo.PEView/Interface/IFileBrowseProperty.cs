using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Interface
{
    /// <summary>
    /// A generic property interface passed as argument to 
    /// <see cref="IFileBrowse"/> interfaces.
    /// </summary>
    public interface IFileBrowseProperty
    {
        /// <summary>
        /// Reader property
        /// </summary>
        IFileReaderProperty ReaderProperty { get; set; }

        /// <summary>
        /// Get or set the reader to parse the file
        /// </summary>
        IFileReadStrategy ReaderStrategy { get; }

        /// <summary>
        /// Data mapping
        /// </summary>
        ICOFFDataStore DataStore { get; set; }
    }
}
