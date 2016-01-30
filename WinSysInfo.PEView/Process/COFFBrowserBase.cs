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

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        public COFFBrowserBase() { }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        /// <param name="reader"></param>
        public COFFBrowserBase(ICOFFReaderProperty reader)
            : this(reader, FactoryCOFFDataStore.Default())
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
        /// Read the file
        /// </summary>
        public void Read()
        {
            if (this.ReaderStrategy == null) throw new ArgumentNullException("ReaderStrategy");
            if (this.DataStore == null) throw new ArgumentNullException("DataStore");
            if (this.ReaderProperty == null) throw new ArgumentNullException("ReaderProperty");

            // Open
            this.ReaderStrategy.Open();

            // Check internal helper
            COFFReaderHelperInternal coffReaderHelper = new COFFReaderHelperInternal(this.ReaderStrategy, this.DataStore);

            // Check if this is a PE/COFF file; if not then return
            if (coffReaderHelper.ContainsPECOFFFileHeader() == false)
                throw new NotSupportedException("This is not PE COFF file");

            // Read the MS DOS Header
            coffReaderHelper.ReadMSDOSHeader();

            // Read the MS DOS Stub
            coffReaderHelper.ReadMSDOSStub();

            // Check the PE magic bytes. ("PE\0\0")
            if (coffReaderHelper.ContainsPEMagicBytes() == false)
                throw new NotImplementedException("This is not PE COFF file");

            // Read NT header
            coffReaderHelper.ReadNTHeader();

            // Check if this normal COFF file or big obj file
            coffReaderHelper.ContainsBigObjHeader();

            if(coffReaderHelper.IsCOFFHeader)
                // Read COFF Header
                coffReaderHelper.ReadCOFFHeader();
            else
                // ReadcoffReaderHelper Big Obj COFF at the same position
                coffReaderHelper.ReadCOFFBigObjHeader();

            // The prior checkSize call may have failed.  This isn't a hard error
            // because we were just trying to sniff out bigobj.
            if (coffReaderHelper.IsCOFFHeader && 
                coffReaderHelper.CoffHeaderObj.IsImportLibrary())
                throw new Exception("Unknown file");

            if (coffReaderHelper.HasPEHeader)
            {
                coffReaderHelper.ReadOptHeaderStdFields();

                // Check PE32 or PE32+
                if (coffReaderHelper.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32)
                {
                    coffReaderHelper.ReadOptHeaderStdFields32();
                    coffReaderHelper.ReadOptHeaderSpecificFields32();
                }
                else if (coffReaderHelper.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32Plus)
                {
                    coffReaderHelper.ReadOptHeaderSpecificFields32Plus();
                }
                else
                {
                    // It's neither PE32 nor PE32+.
                    throw new Exception("It's neither PE32 nor PE32+");
                }

                // Read the data directories
                coffReaderHelper.ReadDataDirectories();

                // Read the Section Table (Section Header)
                coffReaderHelper.ReadSectionTable();
            }

            // Initialize the pointer to the symbol table.
            if (coffReaderHelper.GetPointerToSymbolTable() != 0)
            {
                coffReaderHelper.ReadSymbolTablePointer();
            }
            else
            {
                // We had better not have any symbols if we don't have a symbol table.
                if (coffReaderHelper.GetNumberOfSymbols() != 0)
                    throw new Exception("We had better not have any symbols if we don't have a symbol table");
            }

            // Initialize the pointer to the beginning of the import table.
            if (coffReaderHelper.InitImportTablePointer() == false)
                throw new Exception();

            if (coffReaderHelper.InitDelayImportTablePointer() == false)
                throw new Exception();

            if (coffReaderHelper.InitExportTablePointer() == false)
                throw new Exception();

            if (coffReaderHelper.InitBaseRelocPointer() == false)
                throw new Exception();

            // Find string table. The first four byte of the string table contains the
            // total size of the string table, including the size field itself. If the
            // string table is empty, the value of the first four byte would be 4.

            // Close file
            this.ReaderStrategy.Close();
        }
    }
}
