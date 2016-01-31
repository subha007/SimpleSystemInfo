using System.Collections.Generic;
using System.Linq;
using WinSysInfo.PEView.Factory;
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

        internal void ReadCOFFFile(EnumCOFFFileType fileType)
        {
            ICOFFFileLayoutBrowse browser = FactoryCOFFFileBrowse.Browser(fileType);
            browser.Read(this.ReaderStrategy);
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

        
    }
}
