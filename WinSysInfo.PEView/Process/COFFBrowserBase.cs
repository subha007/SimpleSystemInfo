using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// The link contains PE format parser base https://code.google.com/p/corkami/wiki/PE
    /// </summary>
    public class COFFBrowserBase
    {
        /// <summary>
        /// Use of internal Reader wrapper
        /// </summary>
        private ObjectFileReader Reader { get; set; }

        /// <summary>
        /// Data mapping
        /// </summary>
        public COFFNavigator Navigator { get; set; }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        /// <param name="fullPEFilePath"></param>
        public COFFBrowserBase(string fullPEFilePath)
        {
            this.Reader = new ObjectFileReader(fullPEFilePath);
            this.Navigator = new COFFNavigator();
        }

        #region Reader Helper

        /// <summary>
        /// Stores if this is PE Header or not
        /// </summary>
        protected bool HasPEHeader { get; set;  }

        /// <summary>
        /// Is a COFF Header
        /// </summary>
        protected bool IsCOFFHeader { get; set; }

        protected EnumPEType PEType { get; set; }

        protected LayoutModel<MSDOSHeaderLayout> DosHeaderObj { get; set; }
        protected COFFFileHeaderLayoutModel CoffHeaderObj { get; set; }
        protected LayoutModel<COFFBigObjHeader> CoffBigHeaderObj { get; set; }
        protected LayoutModel<OptionalHeaderStandardFields> OptHStdFields { get; set; }
        protected LayoutModel<OptionalHeaderWindowsSpecificFields32> OptHWinSpFields32 { get; set; }
        protected LayoutModel<OptionalHeaderWindowsSpecificFields32Plus> OptHWinSpFields32Plus { get; set; }

        /// <summary>
        /// Check if the file is PE / COFF
        /// </summary>
        /// <returns></returns>
        protected bool CheckPECOFFFileHeader()
        {
            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    this.Reader.PeekBytes(2))
                                    .ToCharArray();

            this.HasPEHeader = sigchars.SequenceEqual(ConstantWinCOFFImage.MSDOSMagic);
            return this.HasPEHeader;
        }

        /// <summary>
        /// Seek through MS-DOS Header
        /// </summary>
        protected void InitMSDOSHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.DosHeaderObj = this.Reader.ReadLayout<MSDOSHeaderLayout>();
            this.Navigator.SetData(EnumReaderLayoutType.MSDOS_HEADER,
                              this.DosHeaderObj);
        }

        /// <summary>
        /// Seek through MS DOS Stub
        /// </summary>
        protected void InitMSDOSStub()
        {
            MSDOSStubLayoutModel stubObj = new MSDOSStubLayoutModel();
            int size = (int)this.DosHeaderObj.Data.AddressOfNewExeHeader -
                            (int)this.DosHeaderObj.GetOffset("AddressOfNewExeHeader");
            stubObj.SetData(this.Reader.ReadBytes(size));
            this.Navigator.SetData(EnumReaderLayoutType.MSDOS_STUB, stubObj);
        }

        /// <summary>
        /// Check the PE magic bytes. ("PE\0\0")
        /// </summary>
        /// <returns></returns>
        protected bool CheckPEMagicBytes()
        {
            // Check the PE magic bytes. ("PE\0\0")
            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    this.Reader.PeekBytes(2))
                                    .ToCharArray();

            return sigchars.SequenceEqual(ConstantWinCOFFImage.PEMagic);
        }

        /// <summary>
        /// Initialize NT Header
        /// </summary>
        protected void InitNTHeader()
        {
            // Check the PE magic bytes. ("PE\0\0")
            if (this.CheckPEMagicBytes() == true)
            {
                this.Navigator.SetData(EnumReaderLayoutType.NT_HEADER,
                                 this.Reader.ReadLayout<NTHeaderLayout>());
            }
            else
                throw new Exception("Unknown file");
        }

        /// <summary>
        /// Seek through COFF Header
        /// </summary>
        protected void InitCOFFHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.CoffHeaderObj =
                                (COFFFileHeaderLayoutModel)this.Reader.ReadLayout<COFFFileHeader>();
            this.Navigator.SetData(EnumReaderLayoutType.COFF_FILE_HEADER,
                              this.CoffHeaderObj);
        }

        /// <summary>
        /// Seek through COFF Header
        /// </summary>
        protected void InitCOFFBigObjHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.CoffBigHeaderObj =
                                this.Reader.ReadLayout<COFFBigObjHeader>((-1) * LayoutModel<COFFFileHeader>.DataSize);
            this.Navigator.SetData(EnumReaderLayoutType.COFF_BIGOBJ_FILE_HEADER,
                              this.CoffBigHeaderObj);
        }

        /// <summary>
        /// It might be a bigobj file, let's check.  Note that COFF bigobj and COFF
        /// </summary>
        protected void CheckAndInitBigObjHeader()
        {
            bool bPreCoffBigHeader = (!this.IsCOFFHeader &&
                this.CoffHeaderObj.Data.Machine != EnumCOFFHeaderMachineTypes.IMAGE_FILE_MACHINE_UNKNOWN &&
                this.CoffHeaderObj.Data.NumberOfSections == (uint)0xFFFF);

            if (bPreCoffBigHeader == false) return;

            char[] sigchars = System.Text.Encoding.UTF8.GetString(this.CoffBigHeaderObj.Data.UUID)
                                        .ToCharArray();

            if(this.CoffBigHeaderObj.Data.Version >= ConstantWinCOFFImage.MinBigObjectVersion &&
                    sigchars.SequenceEqual(ConstantWinCOFFImage.BigObjMagic) == true)
            {
                this.IsCOFFHeader = false;
            }
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        protected void InitOptHeaderStdFields()
        {
            this.OptHStdFields =
                                this.Reader.ReadLayout<OptionalHeaderStandardFields>();
            this.Navigator.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS,
                              this.OptHStdFields);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        protected void InitOptHeaderStdFields32()
        {
            LayoutModel<OptionalHeaderStandardFields32> obj =
                                this.Reader.ReadLayout<OptionalHeaderStandardFields32>();
            this.Navigator.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS32, obj);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        protected void InitOptHeaderSpecificFields32()
        {
            this.OptHWinSpFields32 =
                                this.Reader.ReadLayout<OptionalHeaderWindowsSpecificFields32>();
            this.Navigator.SetData(EnumReaderLayoutType.OPT_HEADER_SPEC_FLD32, this.OptHWinSpFields32);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        protected void InitOptHeaderSpecificFields32Plus()
        {
            this.OptHWinSpFields32Plus =
                                this.Reader.ReadLayout<OptionalHeaderWindowsSpecificFields32Plus>();
            this.Navigator.SetData(EnumReaderLayoutType.OPT_HEADER_SPEC_FLD32PLUS, this.OptHWinSpFields32Plus);
        }

        /// <summary>
        /// Get Data Directory count
        /// </summary>
        /// <returns></returns>
        protected int GetDataDirectoryCount()
        {
            if (this.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32)
                return (int)this.OptHWinSpFields32.Data.CommonData2.NumberOfRvaAndSizes;
            else if (this.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32Plus)
                return (int)this.OptHWinSpFields32Plus.Data.CommonData2.NumberOfRvaAndSizes;
            else
                return -1;
        }

        protected void InitDataDirectories()
        {
            // Read the data directories
            for (int indx = 0; indx < (int)this.GetDataDirectoryCount(); ++indx)
            {
                EnumReaderLayoutType enumVal = (EnumReaderLayoutType)
                    ((int)EnumReaderLayoutType.OPT_HEADER_DATADIR_EXPORT_TABLE + indx);
                this.Navigator.SetData(enumVal,
                                        this.Reader.ReadLayout<OptionalHeaderDataDirImageOnly>());
            }
        }

        protected int GetNumberOfSections()
        {
            if (this.IsCOFFHeader)
                return this.CoffHeaderObj.IsImportLibrary() ? 0 : this.CoffHeaderObj.Data.NumberOfSections;
            else
                return (int)this.CoffBigHeaderObj.Data.NumberOfSections;
        }

        protected void InitSectionTable()
        {
            List<LayoutModel<COFFSectionTableLayout>> listOfSectionTable =
                            new List<LayoutModel<COFFSectionTableLayout>>();
            for (int indx = 0; indx < (int)this.GetNumberOfSections(); ++indx)
            {
                listOfSectionTable.Add(
                    this.Reader.ReadLayout<COFFSectionTableLayout>());
            }
            this.Navigator.SetData(EnumReaderLayoutType.COFF_SECTION_TABLE,
                                        listOfSectionTable);
        }

        protected int GetPointerToSymbolTable()
        {
            if(this.IsCOFFHeader)
                return this.CoffHeaderObj.IsImportLibrary() ? 0 : (int)this.CoffHeaderObj.Data.PointerToSymbolTable;
            else
                return (int)this.CoffBigHeaderObj.Data.PointerToSymbolTable;
        }

        protected int GetNumberOfSymbols()
        {
            if(this.IsCOFFHeader)
                return this.CoffHeaderObj.IsImportLibrary() ? 0 : (int)this.CoffHeaderObj.Data.NumberOfSymbols;
            else
                return (int)this.CoffBigHeaderObj.Data.NumberOfSymbols;
        }

        protected void InitSymbolTablePointer()
        {
            if(this.IsCOFFHeader)
            {
                List<LayoutModel<COFFSymbolTableLayout>> listOfSymbolTable =
                            new List<LayoutModel<COFFSymbolTableLayout>>();
                for (int indx = 0; indx < (int)this.GetNumberOfSymbols(); ++indx)
                {
                    listOfSymbolTable.Add(
                            this.Reader.ReadLayout<COFFSymbolTableLayout>());
                }
                this.Navigator.SetData(EnumReaderLayoutType.COFF_SYM_TABLE,
                                        listOfSymbolTable);
            }
            else
            {
                List<LayoutModel<COFFSymbolTableBigObjLayout>> listOfSymbolTable =
                            new List<LayoutModel<COFFSymbolTableBigObjLayout>>();
                for (int indx = 0; indx < (int)this.GetNumberOfSymbols(); ++indx)
                {
                    listOfSymbolTable.Add(
                            this.Reader.ReadLayout<COFFSymbolTableBigObjLayout>());
                }
                this.Navigator.SetData(EnumReaderLayoutType.COFF_SYM_TABLE_BIGOBJ,
                                        listOfSymbolTable);
            }
        }

        #endregion

        /// <summary>
        /// Read the file
        /// </summary>
        public void Read()
        {
            // Check if this is PE / COFF
            this.Reader.CreateSequentialAccess();

            // Check if this is a PE/COFF file.
            if (this.CheckPECOFFFileHeader())
            {
                // Read the MS DOS Header and stub
                this.InitMSDOSHeader();
                this.InitMSDOSStub();

                // Check the PE magic bytes. ("PE\0\0")
                this.InitNTHeader();
            }
            else
                throw new Exception("Not a PE File");
            
            // Read COFF Header
            this.InitCOFFHeader();

            // Read Big Obj COFF at the same position
            this.InitCOFFBigObjHeader();

            // It might be a bigobj file, let's check.  Note that COFF bigobj and COFF
            // import libraries share a common prefix but bigobj is more restrictive.
            this.CheckAndInitBigObjHeader();

            // The prior checkSize call may have failed.  This isn't a hard error
            // because we were just trying to sniff out bigobj.
            if (this.IsCOFFHeader && this.CoffHeaderObj.IsImportLibrary())
                throw new Exception("Unknown file");

            // It might be a bigobj file, let's check.  Note that COFF bigobj and COFF
            // import libraries share a common prefix but bigobj is more restrictive.
            //this.IsCOFFHeader = true;
            //uint noOfSections = coffHeaderObj.GetNumberOfSections();
            //uint pointerToSymbolTable = coffHeaderObj.GetPointerToSymbolTable();

            if(this.HasPEHeader)
            {
                this.InitOptHeaderStdFields();

                // Check PE32 or PE32+
                if(this.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32)
                {
                    this.InitOptHeaderStdFields32();
                    this.InitOptHeaderSpecificFields32();
                }
                else if (this.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32Plus)
                {
                    this.InitOptHeaderSpecificFields32Plus();
                }
                else
                {
                    // It's neither PE32 nor PE32+.
                    throw new Exception("It's neither PE32 nor PE32+");
                }

                // Read the data directories
                this.InitDataDirectories();

                // Read the Section Table (Section Header)
                this.InitSectionTable();
            }

            // Initialize the pointer to the symbol table.
            if(this.GetPointerToSymbolTable() != 0)
            {
                this.InitSymbolTablePointer();
            }
            else
            {
                // We had better not have any symbols if we don't have a symbol table.
                if (this.GetNumberOfSymbols() != 0)
                    throw new Exception("We had better not have any symbols if we don't have a symbol table");
            }

            // Find string table. The first four byte of the string table contains the
            // total size of the string table, including the size field itself. If the
            // string table is empty, the value of the first four byte would be 4.

            // Close file
            this.Reader.CloseReader();
        }
    }
}
