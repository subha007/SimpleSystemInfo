using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The layout of PE COFF file.
    /// </summary>
    public class PEFileLayout : COFFDataStore
    {
        /// <summary>
        /// Check if it is PE Header
        /// </summary>
        public bool HasPEHeader { get; set; }

        public bool IsCOFFFileHeader
        {
            get
            {
                return (this.CoffFileHeader != null);
            }
        }

        /// <summary>
        /// The PE DOS Header.
        /// DOS header starts with the first 64 bytes of every PE file. It’s there 
        /// because DOS can recognize it as a valid executable and can run it in the 
        /// DOS stub mode
        /// </summary>
        internal LayoutModel<MSDOSHeaderLayout> MsDosHeader
        {
            get
            {
                return this.GetData<LayoutModel<MSDOSHeaderLayout>>(EnumReaderLayoutType.MSDOS_HEADER);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.MSDOS_HEADER, value);
            }
        }

        /// <summary>
        /// The DOS stub usually just prints a string, something like the message.
        /// It can be a full-blown DOS program. When building applications on Windows,
        /// the linker sends instruction to a binary called winstub.exe to the executable
        /// file. This file is kept in the address 0x3c, which is offset to the next PE 
        /// header section.
        /// </summary>
        internal MSDOSStubLayoutModel MsDosStub
        {
            get
            {
                return this.GetData<MSDOSStubLayoutModel>(EnumReaderLayoutType.MSDOS_STUB);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.MSDOS_STUB, value);
            }
        }

        /// <summary>
        /// NT Header
        /// </summary>
        internal LayoutModel<NTHeaderLayout> NTHeader
        {
            get
            {
                return this.GetData<LayoutModel<NTHeaderLayout>>(EnumReaderLayoutType.NT_HEADER);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.NT_HEADER, value);
            }
        }

        /// <summary>
        /// Like other executable files, a PE file has a collection of fields that defines 
        /// what the rest of file looks like. The header contains info such as the location 
        /// and size of code
        /// </summary>
        internal COFFFileHeaderLayoutModel CoffFileHeader
        {
            get
            {
                return this.GetData<COFFFileHeaderLayoutModel>(EnumReaderLayoutType.COFF_FILE_HEADER);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.COFF_FILE_HEADER, value);
            }
        }

        /// <summary>
        /// Only for Big Object files. The COFF Header.
        /// </summary>
        internal LayoutModel<COFFBigObjHeader> CoffFileBigHeader
        {
            get
            {
                return this.GetData<LayoutModel<COFFBigObjHeader>>(EnumReaderLayoutType.COFF_BIGOBJ_FILE_HEADER);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.COFF_BIGOBJ_FILE_HEADER, value);
            }
        }

        /// <summary>
        /// Optional standard fields
        /// </summary>
        internal LayoutModel<OptionalHeaderStandardFields> OptHStdFields
        {
            get
            {
                return this.GetData<LayoutModel<OptionalHeaderStandardFields>>
                                        (EnumReaderLayoutType.OPT_HEADER_STD_FIELDS);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS, value);
            }
        }

        /// <summary>
        /// Optional standard fields PE32
        /// </summary>
        internal LayoutModel<OptionalHeaderStandardFields32> OptHStdFields32
        {
            get
            {
                return this.GetData<LayoutModel<OptionalHeaderStandardFields32>>
                                        (EnumReaderLayoutType.OPT_HEADER_STD_FIELDS32);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS32, value);
            }
        }

        /// <summary>
        /// If it is PE32 then
        /// </summary>
        internal LayoutModel<OptionalHeaderWindowsSpecificFields32> OptHWinSpFields32
        {
            get
            {
                return this.GetData<LayoutModel<OptionalHeaderWindowsSpecificFields32>>
                                        (EnumReaderLayoutType.OPT_HEADER_STD_FIELDS32);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.OPT_HEADER_STD_FIELDS32, value);
            }
        }

        /// <summary>
        /// If it is PE32+ then
        /// </summary>
        internal LayoutModel<OptionalHeaderWindowsSpecificFields32Plus> OptHWinSpFields32Plus
        {
            get
            {
                return this.GetData<LayoutModel<OptionalHeaderWindowsSpecificFields32Plus>>
                                        (EnumReaderLayoutType.OPT_HEADER_SPEC_FLD32PLUS);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.OPT_HEADER_SPEC_FLD32PLUS, value);
            }
        }

        /// <summary>
        /// Optional directory image
        /// </summary>
        internal LayoutModel<OptionalHeaderDataDirImageOnly> OptHDataDirImportTable
        {
            get
            {
                return this.GetData<LayoutModel<OptionalHeaderDataDirImageOnly>>
                                        (EnumReaderLayoutType.OPT_HEADER_DATADIR_IMPORT_TABLE);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.OPT_HEADER_DATADIR_IMPORT_TABLE, value);
            }
        }

        internal uint NumberOfDataDirImageOnly { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal OptHeaderDataDirectoriesImageOnly OptHDataDirImages
        {
            get
            {
                return this.GetData<OptHeaderDataDirectoriesImageOnly>
                                        (EnumReaderLayoutType.OPT_HEADER_DATADIR_IMAGE_ONLY);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.OPT_HEADER_DATADIR_IMAGE_ONLY, value);
            }
        }

        internal uint NumberOfSections { get; set; }

        internal COFFSectionTableList SectionTables
        {
            get
            {
                return this.GetData<COFFSectionTableList>
                                        (EnumReaderLayoutType.COFF_SECTION_TABLE);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.COFF_SECTION_TABLE, value);
            }
        }

        internal uint PointerToSymbolTable { get; set; }

        internal uint NumberOfSymbols { get; set; }

        internal SymbolTableList SymbolTables
        {
            get
            {
                return this.GetData<SymbolTableList>
                                        (EnumReaderLayoutType.COFF_SYM_TABLE);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.COFF_SYM_TABLE, value);
            }
        }

        internal SymbolTableBigObjList SymbolTables
        {
            get
            {
                return this.GetData<SymbolTableBigObjList>
                                        (EnumReaderLayoutType.COFF_SYM_TABLE_BIGOBJ);
            }
            set
            {
                this.SetData(EnumReaderLayoutType.COFF_SYM_TABLE_BIGOBJ, value);
            }
        }

        internal uint NumberOfImportDirectory;
        internal uint NumberOfDelayImportDirectory;
    }
}
