using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        /// Type of Reader
        /// </summary>
        public COFFReaderProperty ReaderProperty { get; set; }

        /// <summary>
        /// Data mapping
        /// </summary>
        public Dictionary<EnumReaderLayoutType, COFFReaderHandler> FileData { get; set; }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        /// <param name="fullPEFilePath"></param>
        public COFFBrowserBase(string fullPEFilePath)
        {
            this.Reader = new ObjectFileReader(fullPEFilePath);
            this.ReaderProperty = new COFFReaderProperty();
            this.FileData = new Dictionary<EnumReaderLayoutType, COFFReaderHandler>();
        }

        /// <summary>
        /// Read the file
        /// </summary>
        public void Read()
        {
            // Read the DOS Header (always same)
            uint bitwiseFlag = Convert.ToUInt32(this.ReaderProperty.LayoutFlag);
            uint enumValue = 0;

            // Iterate through the enum flags
            for (int count = 0; count < sizeof(EnumReaderLayoutType) - 1; ++count)
            {
                EnumReaderLayoutType layoutType = (EnumReaderLayoutType)(enumValue);

                if (this.ReaderProperty.LayoutFlag.HasFlag(layoutType) == true)
                {
                    COFFReaderHandler handler = this.FactoryHandler(layoutType);
                    handler.Read();
                }
                else
                {
                    enumValue <<= 1;
                }
            }
        }

        /// <summary>
        /// Get the factory method
        /// </summary>
        /// <param name="layoutType"></param>
        /// <returns></returns>
        private COFFReaderHandler FactoryHandler(EnumReaderLayoutType layoutType)
        {
            switch(layoutType)
            {
                case EnumReaderLayoutType.MSDOS_HEADER:
                    return new MSDOSHeaderReaderHandler(EnumReaderLayoutType.MSDOS_HEADER);

                case EnumReaderLayoutType.COFF_FILE_HEADER:
                    return new COFFHeaderReaderHandler(EnumReaderLayoutType.COFF_FILE_HEADER);

            }
                
            return null;
        }
    }
}
