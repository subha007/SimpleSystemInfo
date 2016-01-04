using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The MSDOS stub is a valid application that runs under MSDOS. 
    /// It is placed at the front of the EXE image. The linker places a 
    /// default stub here, which prints out the message “This program 
    /// cannot be run in DOS mode” when the image is run in MSDOS. The 
    /// user can specify a different stub by using the /STUB linker option.
    /// At location 0x3c, the stub has the file offset to the PE signature. 
    /// This information enables Windows to properly execute the image file, 
    /// even though it has an MSDOS stub. This file offset is placed at 
    /// location 0x3c during linking.
    /// </summary>
    /// <remarks>The DOS compatible header at the front of all PEs.</remarks>
    public class MSDOSHeaderLayout
    {
        /// <summary>
        /// Generally ID='MZ'
        /// </summary>
        private char[] magic = new char[2];

        /// <summary>
        /// Generally ID='MZ'
        /// A constant numerical or text value used to identify a file format
        /// <see cref="https://en.wikipedia.org/wiki/List_of_file_signatures"/>
        /// </summary>
        public char[] Magic 
        {
            get { return magic; }
            set { magic = value; }
        }

        /// <summary>
        /// Number of bytes in last 512-byte page of executable
        /// </summary>
        public ushort UsedBytesInTheLastPage { get; set; }

        /// <summary>
        /// Total number of 512-byte pages in executable (including the last page)
        /// </summary>
        public ushort FileSizeInPages { get; set; }

        /// <summary>
        /// Number of relocation entries
        /// </summary>
        public ushort NumberOfRelocationItems { get; set; }

        /// <summary>
        /// Header size in paragraphs
        /// </summary>
        public ushort HeaderSizeInParagraphs { get; set; }

        /// <summary>
        /// Minimum paragraphs of memory allocated in addition to the code size
        /// </summary>
        public ushort MinimumExtraParagraphs { get; set; }

        /// <summary>
        /// Maximum number of paragraphs allocated in addition to the code size
        /// </summary>
        public ushort MaximumExtraParagraphs { get; set; }

        /// <summary>
        /// Initial SS relative to start of executable
        /// </summary>
        public ushort InitialRelativeSS { get; set; }

        /// <summary>
        /// Initial SP
        /// </summary>
        public ushort InitialSP { get; set; }

        /// <summary>
        /// Checksum (or 0) of executable
        /// </summary>
        public ushort Checksum { get; set; }

        /// <summary>
        /// CS:IP relative to start of executable (entry point)
        /// </summary>
        public ushort InitialIP { get; set; }

        /// <summary>
        /// Initial relative CS value
        /// </summary>
        public ushort InitialRelativeCS { get; set; }

        /// <summary>
        /// Offset of relocation table; 40h for new-(NE,LE,LX,W3,PE etc.) 
        /// executable
        /// </summary>
        public ushort AddressOfRelocationTable { get; set; }

        /// <summary>
        /// Overlay number (0h = main program)
        /// </summary>
        public ushort OverlayNumber { get; set; }

        /// <summary>
        /// Reserved words
        /// </summary>
        private ushort[] reserved = new ushort[4];
        public ushort[] Reserved
        {
            get { return this.reserved; }
            set { this.reserved = value; }
        }

        /// <summary>
        /// OEM identifier for <paramref name="OEMinfo"/>
        /// </summary>
        public ushort OEMid { get; set; }

        /// <summary>
        /// OEM information
        /// </summary>
        public ushort OEMinfo { get; set; }

        /// <summary>
        /// Reserved words
        /// </summary>
        private ushort[] reserved2 = new ushort[10];
        public ushort[] Reserved2
        {
            get { return reserved2; }
            set { this.reserved2 = value; }
        }

        /// <summary>
        /// File address of new exe header
        /// </summary>
        public uint AddressOfNewExeHeader { get; set; }
    }
}
