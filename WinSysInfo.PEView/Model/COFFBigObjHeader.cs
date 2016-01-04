using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// bigobj option Increases the number of sections that an object file 
    /// can contain. It increases the address capacity to 4,294,967,296(2**32).
    /// </summary>
    public class COFFBigObjHeader
    {
        /// <summary>
        /// The minimum version number value
        /// </summary>
        public const ushort MinBigObjectVersion = 2;

        /// <summary>
        /// Must be IMAGE_FILE_MACHINE_UNKNOWN (0)
        /// </summary>
        public ushort Sig1 { get; set; }

        /// <summary>
        /// Must be 0xFFFF.
        /// </summary>
        public ushort Sig2 { get; set; }
        public ushort Version { get; set; }
        public ushort Machine { get; set; }
        public uint TimeDateStamp { get; set; }
        private byte[] uuid = new byte[16];
        public byte[] UUID
        {
            get { return uuid; }
            set { uuid = value; }
        }
        public uint Unused1 { get; set; }
        public uint Unused2 { get; set; }
        public uint Unused3 { get; set; }
        public uint Unused4 { get; set; }
        public uint NumberOfSections { get; set; }
        public uint PointerToSymbolTable { get; set; }
        public uint NumberOfSymbols { get; set; }
    }
}
