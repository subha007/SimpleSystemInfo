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
        internal LayoutModel<OptionalHeaderDataDirImageOnly> OptHDataDirImage { get; set; }
        internal uint NumberOfImportDirectory;
        internal uint NumberOfDelayImportDirectory;

        internal EnumCOFFFileType FindCOFFFileType()
        {
            if (PEFileLayoutBrowse.HasPEHeader(ReaderStrategy))
                return EnumCOFFFileType.PE;

            return EnumCOFFFileType.NONE;
        }

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

            //Check if PE header is DWORD-aligned
            if ((this.DosHeaderObj.Data.AddressOfNewExeHeader % sizeof(uint)) != 0)
                throw new System.DataMisalignedException("PE header is not DWORD-aligned");

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
            // Check that the address matches
            if (this.DosHeaderObj.Data.AddressOfNewExeHeader !=
                this.ReaderStrategy.FileOffset)
                throw new System.IndexOutOfRangeException();

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
                                new COFFFileHeaderLayoutModel(this.ReaderStrategy.ReadLayout<COFFFileHeader>());
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

        internal LayoutModel<OptionalHeaderDataDirImageOnly> GetDataDirectory(EnumReaderLayoutType dataDirType)
        {
            if (!(dataDirType >= EnumReaderLayoutType.OPT_HEADER_DATADIR_EXPORT_TABLE &&
                dataDirType <= EnumReaderLayoutType.OPT_HEADER_DATADIR_RESERVED))
                throw new System.ArgumentOutOfRangeException("dataDirType");
            
            return this.DataStore.GetData<LayoutModel<OptionalHeaderDataDirImageOnly>>(dataDirType);
        }

        internal List<LayoutModel<COFFSectionTableLayout>> GetSections()
        {
            return this.DataStore.GetData<List<LayoutModel<COFFSectionTableLayout>>>(
                                                EnumReaderLayoutType.COFF_SECTION_TABLE);
        }

        internal long GetRVAPosition(uint dataRelVirtualAddress)
        {
            List<LayoutModel<COFFSectionTableLayout>> listSections = GetSections();

            foreach (LayoutModel<COFFSectionTableLayout> section in listSections)
            {
                uint sectionEnd = section.Data.VirtualAddress + section.Data.VirtualSize;
                if (section.Data.VirtualAddress <= dataRelVirtualAddress &&
                    dataRelVirtualAddress < sectionEnd)
                {
                    uint offset = dataRelVirtualAddress - section.Data.VirtualAddress;
                    uint position = section.Data.PointerToRawData + offset;
                    return position;
                }
            }

            return -1;
        }

        internal bool InitImportTablePointer()
        {
            // First, we get the RVA of the import table. If the file lacks a pointer to
            // the import table, do nothing.
            this.OptHDataDirImage =
                GetDataDirectory(EnumReaderLayoutType.OPT_HEADER_DATADIR_IMPORT_TABLE);

            // Do nothing if the pointer to import table is NULL.
            if (this.OptHDataDirImage == null) return false;
            if (this.OptHDataDirImage.Data.RelativeVirtualAddress == 0) return false;

            // -1 because the last entry is the null entry.
            this.NumberOfImportDirectory = (this.OptHDataDirImage.Data.Size / LayoutModel<ImportDirectoryTableEntry>.DataSize) - 1;

            // Find the section that contains the RVA. This is needed because the RVA is
            // the import table's memory address which is different from its file offset.
            long positionPtr = GetRVAPosition(this.OptHDataDirImage.Data.RelativeVirtualAddress);

            if (positionPtr < 0) return false;
            this.ReaderStrategy.SeekForward(positionPtr);

            this.DataStore.SetData(EnumReaderLayoutType.IMPORT_DIR_TABLE_ENTRY,
                this.ReaderStrategy.ReadLayout<ImportDirectoryTableEntry>());

            return true;
        }

        internal bool InitDelayImportTablePointer()
        {
            LayoutModel<OptionalHeaderDataDirImageOnly> optHDataDirImage =
                GetDataDirectory(EnumReaderLayoutType.OPT_HEADER_DATADIR_DELAY_IMPORT_DESCRIPTOR);

            if (optHDataDirImage == null) return false;
            if (optHDataDirImage.Data.RelativeVirtualAddress == 0) return false;

            this.NumberOfDelayImportDirectory =
                (optHDataDirImage.Data.Size / LayoutModel<ImportDirectoryTableEntry>.DataSize) - 1;

            long positionPtr = GetRVAPosition(optHDataDirImage.Data.RelativeVirtualAddress);

            if (positionPtr < 0) return false;
            this.ReaderStrategy.SeekForward(positionPtr);

            this.DataStore.SetData(EnumReaderLayoutType.DELAY_IMPORT_DIR_TABLE_ENTRY,
                this.ReaderStrategy.ReadLayout<DelayImportDirectoryTableEntry>());

            return true;
        }

        internal bool InitExportTablePointer()
        {
            LayoutModel<OptionalHeaderDataDirImageOnly> optHDataDirImage =
                GetDataDirectory(EnumReaderLayoutType.OPT_HEADER_DATADIR_EXPORT_TABLE);

            if (optHDataDirImage == null) return false;
            if (optHDataDirImage.Data.RelativeVirtualAddress == 0) return false;

            long positionPtr = GetRVAPosition(optHDataDirImage.Data.RelativeVirtualAddress);

            if (positionPtr < 0) return false;
            this.ReaderStrategy.SeekForward(positionPtr);

            this.DataStore.SetData(EnumReaderLayoutType.EXPORT_DIR_TABLE_ENTRY,
                this.ReaderStrategy.ReadLayout<ExportDirectoryTableEntry>());

            return true;
        }

        internal bool InitBaseRelocPointer()
        {
            LayoutModel<OptionalHeaderDataDirImageOnly> optHDataDirImage =
                GetDataDirectory(EnumReaderLayoutType.OPT_HEADER_DATADIR_BASE_RELOCATION_TABLE);

            if (optHDataDirImage == null) return false;
            if (optHDataDirImage.Data.RelativeVirtualAddress == 0) return false;

            long positionPtr = GetRVAPosition(optHDataDirImage.Data.RelativeVirtualAddress);

            if (positionPtr < 0) return false;
            this.ReaderStrategy.SeekForward(positionPtr);

            this.DataStore.SetData(EnumReaderLayoutType.BASE_RELOC_HEADER,
                this.ReaderStrategy.ReadLayout<COFFBaseRelocBlockHeader>());

            this.ReaderStrategy.SeekForward(optHDataDirImage.Data.Size);

            this.DataStore.SetData(EnumReaderLayoutType.BASE_RELOC_END,
                this.ReaderStrategy.ReadLayout<COFFBaseRelocBlockHeader>());

            return true;
        }
    }
}
