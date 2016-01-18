using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// These are common fields for 32 and 32+ extension to the COFF optional header format.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct OptionalHeaderWindowsSpecificFieldsCommon2
    {
        /// <summary>
        /// Reserved, must be zero
        /// </summary>
        public uint LoaderFlags { get; set; }

        /// <summary>
        /// The number of data-directory entries in the remainder of the optional header. 
        /// Each describes a location and size
        /// </summary>
        public uint NumberOfRvaAndSizes { get; set; }
    }
}
