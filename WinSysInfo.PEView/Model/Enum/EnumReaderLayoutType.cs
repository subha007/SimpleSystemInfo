using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The flags representing individual structures
    /// The enums are to be in sequential order of file read
    /// </summary>
    [Flags]
    public enum EnumReaderLayoutType : ulong
    {
        NONE = 0x00000000,
        MSDOS_HEADER = 0x00000001,
        NT_HEADER = 0x00000002,
        COFF_FILE_HEADER = 0x00000002,

        FULL_READ = 0xFFFFFFFF,
    }
}
