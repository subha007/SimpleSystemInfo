using System.Collections.Generic;
using System.Linq;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// Internal helper class
    /// </summary>
    internal class COFFReaderHelperInternal
    {
        /// <summary>
        /// Use of internal Reader wrapper
        /// </summary>
        private IFileReadStrategy ReaderStrategy { get; set; }

        /// <summary>
        /// Data mapping
        /// </summary>
        internal ICOFFDataStore DataStore { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="strategy"></param>
        /// <param name="store"></param>
        internal COFFReaderHelperInternal(IFileReadStrategy strategy, ICOFFDataStore store)
        {
            this.ReaderStrategy = strategy;
            this.DataStore = store;
        }

        /// <summary>
        /// Stores if this is PE Header or not
        /// </summary>
        internal bool HasPEHeader { get; set; }
        internal bool IsCOFFHeader { get; set; }

        internal LayoutModel<MSDOSHeaderLayout> DosHeaderObj { get; set; }
        internal MSDOSStubLayoutModel DosStubObj { get; set; }
        internal COFFFileHeaderLayoutModel CoffHeaderObj { get; set; }
        internal LayoutModel<COFFBigObjHeader> CoffBigHeaderObj { get; set; }
        internal LayoutModel<OptionalHeaderStandardFields> OptHStdFields { get; set; }
        internal LayoutModel<OptionalHeaderWindowsSpecificFields32> OptHWinSpFields32 { get; set; }
        internal LayoutModel<OptionalHeaderWindowsSpecificFields32Plus> OptHWinSpFields32Plus { get; set; }

        /// <summary>
        /// Check if the file is PE / COFF
        /// </summary>
        /// <returns></returns>
        internal bool ContainsPECOFFFileHeader()
        {
            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    this.ReaderStrategy.PeekBytes(2, -1))
                                    .ToCharArray();

            this.HasPEHeader = sigchars.SequenceEqual(ConstantWinCOFFImage.MSDOSMagic);
            return this.HasPEHeader;
        }

        /// <summary>
        /// Seek through MS-DOS Header
        /// </summary>
        internal void ReadMSDOSHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.DosHeaderObj = this.ReaderStrategy.ReadLayout<MSDOSHeaderLayout>();
            this.DataStore.SetData(EnumReaderLayoutType.MSDOS_HEADER,
                              this.DosHeaderObj);
        }

        /// <summary>
        /// Seek through MS DOS Stub
        /// </summary>
        internal void ReadMSDOSStub()
        {
            this.DosStubObj = new MSDOSStubLayoutModel();
            int size = (int)this.DosHeaderObj.Data.AddressOfNewExeHeader -
                            (int)this.ReaderStrategy.FileOffset;
            this.DosStubObj.SetData(this.ReaderStrategy.ReadBytes(size));
            this.DataStore.SetData(EnumReaderLayoutType.MSDOS_STUB, this.DosStubObj);
        }

        /// <summary>
        /// Check the PE magic bytes. ("PE\0\0")
        /// </summary>
        /// <returns></returns>
        internal bool ContainsPEMagicBytes()
        {
            // Check the PE magic bytes. ("PE\0\0")
            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    this.ReaderStrategy.PeekBytes(4, -1))
                                    .ToCharArray();

            return sigchars.SequenceEqual(ConstantWinCOFFImage.PEMagic);
        }

        /// <summary>
        /// Initialize NT Header
        /// </summary>
        internal void ReadNTHeader()
        {
            this.DataStore.SetData(EnumReaderLayoutType.NT_HEADER,
                                 this.ReaderStrategy.ReadLayout<NTHeaderLayout>());
        }

        /// <summary>
        /// Seek through COFF Header
        /// </summary>
        internal void ReadCOFFHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.CoffHeaderObj =
                                (COFFFileHeaderLayoutModel) this.ReaderStrategy.ReadLayout<COFFFileHeader>();
            this.DataStore.SetData(EnumReaderLayoutType.COFF_FILE_HEADER,
                              this.CoffHeaderObj);
        }

        /// <summary>
        /// Seek through COFF Header
        /// </summary>
        internal void ReadCOFFBigObjHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.CoffBigHeaderObj =
                   this.ReaderStrategy.ReadLayout<COFFBigObjHeader>((-1) * LayoutModel<COFFFileHeader>.DataSize);
            this.DataStore.SetData(EnumReaderLayoutType.COFF_BIGOBJ_FILE_HEADER,
                   this.CoffBigHeaderObj);
        }

        /// <summary>
        /// It might be a bigobj file, let's check.  Note that COFF bigobj and COFF
        /// </summary>
        internal void ContainsBigObjHeader()
        {
            this.IsCOFFHeader = true;
            bool bPreCoffBigHeader = (!this.HasPEHeader &&
                this.ReaderStrategy.PeekUShort(COFFFileHeaderLayoutModel.GetOffset("Machine")) == (ushort)0 &&
                this.ReaderStrategy.PeekUShort(COFFFileHeaderLayoutModel.GetOffset("NumberOfSections")) == (ushort)0xFFFF);

            if (bPreCoffBigHeader == false) return;

            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                this.ReaderStrategy.ReadBytes(16, LayoutModel<COFFBigObjHeader>.GetOffset("UUID")))
                .ToCharArray();

            if (this.ReaderStrategy.PeekUShort(LayoutModel<COFFBigObjHeader>.GetOffset("Version"))
                >= ConstantWinCOFFImage.MinBigObjectVersion &&
                    sigchars.SequenceEqual(ConstantWinCOFFImage.BigObjMagic) == true)
            {
                this.IsCOFFHeader = false;
            }
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderStdFields()
        {
            this.OptHStdFields =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderStandardFields>();
            this.DataStore.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS,
                              this.OptHStdFields);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderStdFields32()
        {
            LayoutModel<OptionalHeaderStandardFields32> obj =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderStandardFields32>();
            this.DataStore.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS32, obj);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderSpecificFields32()
        {
            this.OptHWinSpFields32 =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderWindowsSpecificFields32>();
            this.DataStore.SetData(EnumReaderLayoutType.OPT_HEADER_SPEC_FLD32, this.OptHWinSpFields32);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderSpecificFields32Plus()
        {
            this.OptHWinSpFields32Plus =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderWindowsSpecificFields32Plus>();
            this.DataStore.SetData(EnumReaderLayoutType.OPT_HEADER_SPEC_FLD32PLUS, this.OptHWinSpFields32Plus);
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

        internal void ReadDataDirectories()
        {
            // Read the data directories
            for (int indx = 0; indx < (int)this.GetDataDirectoryCount(); ++indx)
            {
                EnumReaderLayoutType enumVal = (EnumReaderLayoutType)
                    ((int)EnumReaderLayoutType.OPT_HEADER_DATADIR_EXPORT_TABLE + indx);
                this.DataStore.SetData(enumVal,
                                        this.ReaderStrategy.ReadLayout<OptionalHeaderDataDirImageOnly>());
            }
        }

        internal int GetNumberOfSections()
        {
            if (this.IsCOFFHeader)
                return this.CoffHeaderObj.IsImportLibrary() ? 0 : this.CoffHeaderObj.Data.NumberOfSections;
            else
                return (int)this.CoffBigHeaderObj.Data.NumberOfSections;
        }

        internal void ReadSectionTable()
        {
            List<LayoutModel<COFFSectionTableLayout>> listOfSectionTable =
                            new List<LayoutModel<COFFSectionTableLayout>>();
            for (int indx = 0; indx < (int)this.GetNumberOfSections(); ++indx)
            {
                listOfSectionTable.Add(
                    this.ReaderStrategy.ReadLayout<COFFSectionTableLayout>());
            }
            this.DataStore.SetData(EnumReaderLayoutType.COFF_SECTION_TABLE,
                                        listOfSectionTable);
        }

        internal int GetPointerToSymbolTable()
        {
            if (this.IsCOFFHeader)
                return this.CoffHeaderObj.IsImportLibrary() ? 0 : (int)this.CoffHeaderObj.Data.PointerToSymbolTable;
            else
                return (int)this.CoffBigHeaderObj.Data.PointerToSymbolTable;
        }

        internal int GetNumberOfSymbols()
        {
            if (this.IsCOFFHeader)
                return this.CoffHeaderObj.IsImportLibrary() ? 0 : (int)this.CoffHeaderObj.Data.NumberOfSymbols;
            else
                return (int)this.CoffBigHeaderObj.Data.NumberOfSymbols;
        }

        internal void ReadSymbolTablePointer()
        {
            if (this.IsCOFFHeader)
            {
                List<COFFSymbolTableLayoutModel> listOfSymbolTable =
                            new List<COFFSymbolTableLayoutModel>();
                for (int indx = 0; indx < (int)this.GetNumberOfSymbols(); ++indx)
                {
                    listOfSymbolTable.Add((COFFSymbolTableLayoutModel)
                            this.ReaderStrategy.ReadLayout<COFFSymbolTableLayout>());
                }
                this.DataStore.SetData<List<COFFSymbolTableLayoutModel>>(EnumReaderLayoutType.COFF_SYM_TABLE,
                                        listOfSymbolTable);
            }
            else
            {
                List<LayoutModel<COFFSymbolTableBigObjLayout>> listOfSymbolTable =
                            new List<LayoutModel<COFFSymbolTableBigObjLayout>>();
                for (int indx = 0; indx < (int)this.GetNumberOfSymbols(); ++indx)
                {
                    listOfSymbolTable.Add(
                            this.ReaderStrategy.ReadLayout<COFFSymbolTableBigObjLayout>());
                }
                this.DataStore.SetData(EnumReaderLayoutType.COFF_SYM_TABLE_BIGOBJ,
                                        listOfSymbolTable);
            }
        }

        internal void GetDataDirectory(EnumReaderLayoutType dataDirType)
        {
            LayoutModel<OptionalHeaderDataDirImageOnly> dataDirPointer =
                this.DataStore.GetData<LayoutModel<OptionalHeaderDataDirImageOnly>>(dataDirType);
            //this.ReaderStrategy.()
        }

        internal void InitImportTablePointer()
        {

        }
    }
}
