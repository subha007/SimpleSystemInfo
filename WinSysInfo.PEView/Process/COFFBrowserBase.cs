using System;
using WinSysInfo.PEView.Factory;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// This class can browse any PE/COFF file and extract the informations in a Data Store
    /// This class is 
    /// The link contains PE format parser base https://code.google.com/p/corkami/wiki/PE
    /// </summary>
    public class COFFBrowserBase : ICOFFBrowser
    {
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
        private ICOFFReaderProperty readerProperty;
        public ICOFFReaderProperty ReaderProperty 
        {
            get { return readerProperty; }
            set
            {
                this.readerProperty = value;
                this.ReaderStrategy = FactoryFileReadStrategy.Instance(this.readerProperty);
            }
        }

        public EnumCOFFFileType TypeOfCOFF { get; set; }

        public ICOFFFileLayout FileLayout { get; set; }

        public ICOFFFileLayoutBrowse FileLayoutBrowse { get; set; }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        public COFFBrowserBase() { }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        /// <param name="reader"></param>
        public COFFBrowserBase(ICOFFReaderProperty reader)
            : this(reader, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="store"></param>
        public COFFBrowserBase(ICOFFReaderProperty reader, ICOFFDataStore store)
        {
            this.ReaderProperty = reader;
            this.DataStore = store;
        }

        /// <summary>
        /// Find the type of file
        /// </summary>
        /// <returns></returns>
        internal EnumCOFFFileType FindCOFFFileType()
        {
            if (PEFileLayoutBrowse.HasPEHeader(ReaderStrategy))
                return EnumCOFFFileType.PE;

            return EnumCOFFFileType.NONE;
        }

        /// <summary>
        /// Read the file
        /// </summary>
        public void Read()
        {
            if (this.ReaderStrategy == null) throw new ArgumentNullException("ReaderStrategy");
            if (this.ReaderProperty == null) throw new ArgumentNullException("ReaderProperty");

            // Open
            this.ReaderStrategy.Open();
            
            // Get file type
            this.TypeOfCOFF = this.FindCOFFFileType();

            // Check if this is a PE/COFF file; if not then return
            if (this.TypeOfCOFF == EnumCOFFFileType.NONE)
                throw new NotSupportedException("This is not COFF file");

            this.FileLayoutBrowse = FactoryCOFFFileBrowse.Browser(this.TypeOfCOFF, this.ReaderStrategy);

            this.FileLayoutBrowse.Read(this.ReaderStrategy);

            

            // Close file
            this.ReaderStrategy.Close();
        }
    }
}
