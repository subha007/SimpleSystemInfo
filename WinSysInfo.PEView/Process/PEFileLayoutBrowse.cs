using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Factory;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// This is an internal class used by <see cref="COFFBrowserBase"/> class
    /// The COFF file is already identified as a PE file. This class is used to
    /// browse the PE file.
    /// </summary>
    internal class PEFileLayoutBrowse : ICOFFFileLayoutBrowse
    {
        /// <summary>
        /// The reader used to read the PE File 
        /// </summary>
        private IFileReadStrategy ReaderStrategy { get; set; }

        /// <summary>
        /// The data store to store the parsed file data
        /// </summary>
        internal PEFileLayout DataStore { get; set; }

        /// <summary>
        /// Get or set the PE type 32 bit or 64 bit
        /// </summary>
        internal EnumPEType PEType { get; set; }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="readerStrategy"></param>
        internal PEFileLayoutBrowse(IFileReadStrategy readerStrategy)
        {
            this.ReaderStrategy = readerStrategy;
            this.DataStore = (PEFileLayout) FactoryCOFFDataStore.PEStore();
        }

        /// <summary>
        /// Check if the file has PE signature
        /// </summary>
        /// <param name="ReaderStrategy"></param>
        /// <returns></returns>
        internal static bool HasPEHeader(IFileReadStrategy ReaderStrategy)
        {
            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    ReaderStrategy.PeekBytes(2, -1))
                                    .ToCharArray();

            return sigchars.SequenceEqual(ConstantWinCOFFImage.MSDOSMagic);
        }

        /// <summary>
        /// Calculate the PE Type
        /// </summary>
        /// <param name="ReaderStrategy"></param>
        /// <returns></returns>
        internal static EnumPEType GetPEType(IFileReadStrategy ReaderStrategy)
        {
            // Calculate the position in the file to find the PE type magic
            uint posAddrNewHeader = LayoutModel<MSDOSHeaderLayout>.DataSize -
                                (uint)LayoutModel<MSDOSHeaderLayout>.GetOffset("AddressOfNewExeHeader");

            uint addrNewHeader = ReaderStrategy.PeekUInt(posAddrNewHeader);

            uint posPEMagic = addrNewHeader + LayoutModel<NTHeaderLayout>.DataSize
                                + COFFFileHeaderLayoutModel.DataSize;

            return (EnumPEType)ReaderStrategy.PeekUShort(posPEMagic);
        }

        /// <summary>
        /// Seek through MS-DOS Header
        /// </summary>
        internal void ReadMSDOSHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.DataStore.MsDosHeader = this.ReaderStrategy.ReadLayout<MSDOSHeaderLayout>();

            //Check if PE header is DWORD-aligned
            if ((this.DataStore.MsDosHeader.Data.AddressOfNewExeHeader % sizeof(uint)) != 0)
                throw new System.DataMisalignedException("PE header is not DWORD-aligned");
        }

        /// <summary>
        /// Seek through MS DOS Stub
        /// </summary>
        internal void ReadMSDOSStub()
        {
            MSDOSStubLayoutModel dosStubObj = new MSDOSStubLayoutModel();
            int size = (int)this.DataStore.MsDosHeader.Data.AddressOfNewExeHeader -
                            (int)this.ReaderStrategy.FileOffset;
            dosStubObj.SetData(this.ReaderStrategy.ReadBytes(size));

            this.DataStore.MsDosStub = dosStubObj;
        }

        /// <summary>
        /// Check the PE magic bytes. ("PE\0\0")
        /// </summary>
        /// <returns></returns>
        internal bool ContainsPEMagicBytes()
        {
            // Check that the address matches
            if (this.DataStore.MsDosHeader.Data.AddressOfNewExeHeader !=
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
            this.DataStore.NTHeader = this.ReaderStrategy.ReadLayout<NTHeaderLayout>();
        }

        /// <summary>
        /// Seek through COFF Header
        /// </summary>
        internal void ReadCOFFHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.DataStore.CoffFileHeader =
                                new COFFFileHeaderLayoutModel(this.ReaderStrategy.ReadLayout<COFFFileHeader>());
        }

        /// <summary>
        /// Seek through COFF Header
        /// </summary>
        internal void ReadCOFFBigObjHeader()
        {
            // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
            // PE signature to find 'normal' COFF header.
            this.DataStore.CoffFileBigHeader =
                   this.ReaderStrategy.ReadLayout<COFFBigObjHeader>((-1) * LayoutModel<COFFFileHeader>.DataSize);
        }

        /// <summary>
        /// It might be a bigobj file, let's check.  Note that COFF bigobj and COFF
        /// </summary>
        internal void CheckAndMaintainBigObjHeader()
        {
            if ((!this.DataStore.HasPEHeader &&
                this.ReaderStrategy.PeekUShort(COFFFileHeaderLayoutModel.GetOffset("Machine")) == (ushort)0 &&
                this.ReaderStrategy.PeekUShort(COFFFileHeaderLayoutModel.GetOffset("NumberOfSections")) == (ushort)0xFFFF)
                == false)
                return;

            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                this.ReaderStrategy.ReadBytes(16, LayoutModel<COFFBigObjHeader>.GetOffset("UUID")))
                .ToCharArray();

            if (this.ReaderStrategy.PeekUShort(LayoutModel<COFFBigObjHeader>.GetOffset("Version"))
                >= ConstantWinCOFFImage.MinBigObjectVersion &&
                    sigchars.SequenceEqual(ConstantWinCOFFImage.BigObjMagic) == true)
                this.DataStore.Delete(EnumReaderLayoutType.COFF_FILE_HEADER);
            else
                this.DataStore.Delete(EnumReaderLayoutType.COFF_BIGOBJ_FILE_HEADER);
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderStdFields()
        {
            this.DataStore.OptHStdFields =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderStandardFields>();
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderStdFields32()
        {
            this.DataStore.OptHStdFields32 =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderStandardFields32>();
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderSpecificFields32()
        {
            this.DataStore.OptHWinSpFields32 =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderWindowsSpecificFields32>();
        }

        /// <summary>
        /// Initialize optional header standard fields
        /// </summary>
        internal void ReadOptHeaderSpecificFields32Plus()
        {
            this.DataStore.OptHWinSpFields32Plus =
                                this.ReaderStrategy.ReadLayout<OptionalHeaderWindowsSpecificFields32Plus>();
        }

        /// <summary>
        /// Get Data Directory count
        /// </summary>
        /// <returns></returns>
        protected void CalculateDataDirectoryCount()
        {
            if (this.DataStore.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32)
                this.DataStore.NumberOfDataDirImageOnly =
                    this.DataStore.OptHWinSpFields32.Data.CommonData2.NumberOfRvaAndSizes;
            else if (this.DataStore.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32Plus)
                this.DataStore.NumberOfDataDirImageOnly =
                    this.DataStore.OptHWinSpFields32Plus.Data.CommonData2.NumberOfRvaAndSizes;
            else
                this.DataStore.NumberOfDataDirImageOnly = 0;
        }

        /// <summary>
        /// Each data directory gives the address and size of a table or string that Windows uses.
        /// These data directory entries are all loaded into memory so that the system can use them
        /// at run time. A data directory is an 8byte field.
        /// </summary>
        internal void ReadOptHeaderDataDirectoriesImageOnly()
        {
            // Read the data directories
            OptHeaderDataDirectoriesImageOnly dirsImage = new OptHeaderDataDirectoriesImageOnly();
            
            for (int indx = 0; indx < this.DataStore.NumberOfDataDirImageOnly; ++indx)
            {
                EnumReaderLayoutType enumVal = (EnumReaderLayoutType)
                    ((int)EnumReaderLayoutType.OPT_HEADER_DATADIR_EXPORT_TABLE + indx);
                dirsImage.Add(enumVal,
                                        this.ReaderStrategy.ReadLayout<OptionalHeaderDataDirImageOnly>());
            }

            this.DataStore.OptHDataDirImages = dirsImage;
        }

        /// <summary>
        /// Get the count of sections
        /// </summary>
        /// <returns></returns>
        internal void CalculateNumberOfSections()
        {
            if (this.DataStore.IsCOFFFileHeader)
                this.DataStore.NumberOfSections =  
                    (uint)(this.DataStore.CoffFileHeader.IsImportLibrary() ? 
                                                        0 : this.DataStore.CoffFileHeader.Data.NumberOfSections);
            else
                this.DataStore.NumberOfSections = this.DataStore.CoffFileBigHeader.Data.NumberOfSections;
        }

        /// <summary>
        /// Read the section tables list
        /// </summary>
        internal void ReadSectionTable()
        {
            COFFSectionTableList listOfSectionTable = new COFFSectionTableList();
            for (int indx = 0; indx < this.DataStore.NumberOfSections; ++indx)
            {
                listOfSectionTable.Add(
                    this.ReaderStrategy.ReadLayout<COFFSectionTableLayout>());
            }
            this.DataStore.SectionTables = listOfSectionTable;
        }

        /// <summary>
        /// Calculate the position of the symbol table in the file
        /// </summary>
        internal void CalculatePointerToSymbolTable()
        {
            if (this.DataStore.IsCOFFFileHeader)
                this.DataStore.PointerToSymbolTable =
                    this.DataStore.CoffFileHeader.IsImportLibrary() ?
                            0 : this.DataStore.CoffFileHeader.Data.PointerToSymbolTable;
            else
                this.DataStore.PointerToSymbolTable = 
                    this.DataStore.CoffFileBigHeader.Data.PointerToSymbolTable;
        }

        /// <summary>
        /// Calculate the number of symbols
        /// </summary>
        internal void CalculateNumberOfSymbols()
        {
            if (this.DataStore.IsCOFFFileHeader)
                this.DataStore.NumberOfSymbols =
                    this.DataStore.CoffFileHeader.IsImportLibrary() ? 
                    0 : this.DataStore.CoffFileHeader.Data.NumberOfSymbols;
            else
                this.DataStore.NumberOfSymbols = this.DataStore.CoffFileBigHeader.Data.NumberOfSymbols;
        }

        /// <summary>
        /// Read the symbol table pointer
        /// </summary>
        internal void ReadSymbolTablePointer()
        {
            if (this.DataStore.IsCOFFFileHeader)
            {
                SymbolTableList listOfSymbolTable = new SymbolTableList();
                for (int indx = 0; indx < (int)this.DataStore.NumberOfSymbols; ++indx)
                    listOfSymbolTable.Add(new COFFSymbolTableLayoutModel(
                                this.ReaderStrategy.ReadLayout<COFFSymbolTableLayout>()));
                this.DataStore.SetData<List<COFFSymbolTableLayoutModel>>(EnumReaderLayoutType.COFF_SYM_TABLE,
                                        listOfSymbolTable);
            }
            else
            {
                SymbolTableBigObjList listOfSymbolTable = new SymbolTableBigObjList();
                for (int indx = 0; indx < (int)this.DataStore.NumberOfSymbols; ++indx)
                    listOfSymbolTable.Add(
                                this.ReaderStrategy.ReadLayout<COFFSymbolTableBigObjLayout>());
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

        internal void Read(IFileReadStrategy readStrategy)
        {
            // Read the MS DOS Header
            ReadMSDOSHeader();

            // Read the MS DOS Stub
            ReadMSDOSStub();

            // Check the PE magic bytes. ("PE\0\0")
            if (ContainsPEMagicBytes() == false)
                throw new NotImplementedException("This is not PE COFF file");

            // Read NT header
            ReadNTHeader();

            // Check if this normal COFF file or big obj file
            CheckAndMaintainBigObjHeader();

            if (this.DataStore.IsCOFFFileHeader)
                // Read COFF Header
                ReadCOFFHeader();
            else
                // ReadcoffReaderHelper Big Obj COFF at the same position
                ReadCOFFBigObjHeader();

            // The prior checkSize call may have failed.  This isn't a hard error
            // because we were just trying to sniff out bigobj.
            if (this.DataStore.IsCOFFFileHeader && this.DataStore.CoffFileHeader.IsImportLibrary())
                throw new Exception("Unknown file");

            if (this.DataStore.HasPEHeader)
            {
                ReadOptHeaderStdFields();

                // Check PE32 or PE32+
                if (this.DataStore.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32)
                {
                    ReadOptHeaderStdFields32();
                    ReadOptHeaderSpecificFields32();
                }
                else if (this.DataStore.OptHStdFields.Data.Magic == EnumOptionalHeaderMagicNo.PE32Plus)
                {
                    ReadOptHeaderSpecificFields32Plus();
                }
                else
                {
                    // It's neither PE32 nor PE32+.
                    throw new Exception("It's neither PE32 nor PE32+");
                }

                // Read the data directories
                ReadOptHeaderDataDirectoriesImageOnly();

                // Read the Section Table (Section Header)
                CalculateNumberOfSections();
                ReadSectionTable();
            }

            // Initialize the pointer to the symbol table.
            CalculateDataDirectoryCount();
            if (this.DataStore.NumberOfDataDirImageOnly != 0)
            {
                ReadSymbolTablePointer();
            }
            else
            {
                // We had better not have any symbols if we don't have a symbol table.
                if (GetNumberOfSymbols() != 0)
                    throw new Exception("We had better not have any symbols if we don't have a symbol table");
            }

            // Initialize the pointer to the beginning of the import table.
            if (InitImportTablePointer() == false)
                throw new Exception();

            if (InitDelayImportTablePointer() == false)
                throw new Exception();

            if (InitExportTablePointer() == false)
                throw new Exception();

            if (InitBaseRelocPointer() == false)
                throw new Exception();

            // Find string table. The first four byte of the string table contains the
            // total size of the string table, including the size field itself. If the
            // string table is empty, the value of the first four byte would be 4.
        }
    }
}
