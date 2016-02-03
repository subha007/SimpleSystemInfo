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
    public class FileBrowserProperty
    {
        /// <summary>
        /// The type of file
        /// </summary>
        public EnumFileType FileType { get; set; }

        /// <summary>
        /// Get or set the file path data
        /// </summary>
        public HugeFilePathHelper FilePathData { get; protected set; }

        /// <summary>
        /// Use of internal Reader wrapper
        /// </summary>
        public IFileReadStrategy ReaderStrategy { get; protected set; }

        /// <summary>
        /// Data mapping
        /// </summary>
        public ICOFFDataStore DataStore { get; set; }

        /// <summary>
        /// Reader property
        /// </summary>
        private IFileReaderProperty readerProperty;
        public IFileReaderProperty ReaderProperty
        {
            get { return readerProperty; }
            set
            {
                this.readerProperty = value;
                this.ReaderStrategy = FactoryFileReadStrategy.Instance(this.readerProperty);
            }
        }
    }
}
