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

        /// <summary>
        /// Read the file
        /// </summary>
        public void Read()
        {
            // Check if this is PE / COFF
            this.Reader.CreateSequentialAccess();

            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    this.Reader.PeekBytes(2))
                                    .ToCharArray();

            bool bHasPEHeader = false;
            if(sigchars.SequenceEqual(ConstantWinCOFFImage.MSDOSMagic) == true)
            {
                // PE/COFF, seek through MS-DOS compatibility stub and 4-byte
                // PE signature to find 'normal' COFF header.
                this.Navigator.SetData(EnumReaderLayoutType.MSDOS_HEADER,
                                  this.Reader.ReadLayout<MSDOSHeaderLayout>());

                // Read the MS DOS Stub
                LayoutModel<MSDOSHeaderLayout> dosHeaderbObj = 
                    this.Navigator.GetData<MSDOSHeaderLayout>(EnumReaderLayoutType.MSDOS_HEADER);

                MSDOSStubLayoutModel stubObj = new MSDOSStubLayoutModel();
                int size = (int)dosHeaderbObj.Data.AddressOfNewExeHeader - (int)dosHeaderbObj.GetOffset("AddressOfNewExeHeader");
                stubObj.SetData(this.Reader.ReadBytes(size));
                this.Navigator.SetData(EnumReaderLayoutType.MSDOS_STUB, stubObj);

                // Check the PE magic bytes. ("PE\0\0")
                // Check if this is a PE/COFF file.
                sigchars = System.Text.Encoding.UTF8.GetString(
                                        this.Reader.PeekBytes(2))
                                        .ToCharArray();
                if(sigchars.SequenceEqual(ConstantWinCOFFImage.PEMagic) == true)
                {
                    this.Navigator.SetData(EnumReaderLayoutType.NT_HEADER,
                                     this.Reader.ReadLayout<NTHeaderLayout>());
                }

                bHasPEHeader = true;
            }
            
            // Read COFF Header
            this.Navigator.SetData(EnumReaderLayoutType.COFF_FILE_HEADER,
                                     this.Reader.ReadLayout<COFFFileHeader>());

            // It might be a bigobj file, let's check.  Note that COFF bigobj and COFF
            // import libraries share a common prefix but bigobj is more restrictive.
            LayoutModel<COFFFileHeader> coffHeaderbObj =
                this.Navigator.GetData<COFFFileHeader>(EnumReaderLayoutType.COFF_FILE_HEADER);
            if(!bHasPEHeader && 
                coffHeaderbObj.Data.Machine != EnumCOFFHeaderMachineTypes.IMAGE_FILE_MACHINE_UNKNOWN &&
                coffHeaderbObj.Data.NumberOfSections == (uint)0xFFFF)
            { 
            }
        }
    }
}
