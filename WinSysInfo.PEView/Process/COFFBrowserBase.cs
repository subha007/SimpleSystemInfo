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
        public Dictionary<EnumReaderLayoutType, object> FileData { get; set; }

        /// <summary>
        /// Construct the base PE browser
        /// </summary>
        /// <param name="fullPEFilePath"></param>
        public COFFBrowserBase(string fullPEFilePath)
        {
            this.Reader = new ObjectFileReader(fullPEFilePath);
            this.FileData = new Dictionary<EnumReaderLayoutType, object>();
        }

        /// <summary>
        /// Read the file
        /// </summary>
        public void Read()
        {
            // Check if this is PE / COFF
            this.Reader.CreateSequentialAccess(0, LayoutModel<MSDOSHeaderLayout>.DataSize +
                                            LayoutModel<NTHeaderLayout>.DataSize);

            // Check if this is a PE/COFF file.
            byte[] sigBytes = this.Reader.ReadBytes(0, 2);
            char[] sigchars = System.Text.Encoding.UTF8.GetString(sigBytes).ToCharArray();

            if(sigchars.SequenceEqual(ConstantWinCOFFImage.MSDOSMagic) == true)
            {
                // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
                // PE signature to find 'normal' COFF header.
                this.FileData.Add(EnumReaderLayoutType.MSDOS_HEADER,
                    (LayoutModel<MSDOSHeaderLayout>) this.Reader.ReadLayout<MSDOSHeaderLayout>(0));

                // Check the PE magic bytes. ("PE\0\0")
                this.FileData.Add(EnumReaderLayoutType.NT_HEADER,
                    (LayoutModel<NTHeaderLayout>) this.Reader.ReadLayout<NTHeaderLayout>(0));

                NTHeaderLayout model = (NTHeaderLayout)(this.FileData[EnumReaderLayoutType.NT_HEADER]);
                if(model.Signature.SequenceEqual(ConstantWinCOFFImage.PEMagic) == false)
                    return;
            }

            // Read COFF header
        }
    }
}
