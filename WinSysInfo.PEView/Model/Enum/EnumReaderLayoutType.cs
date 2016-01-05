using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    public enum EnumReaderLayoutType : ulong
    {
        NONE = 0x00000000,
        MSDOS_HEADER = 0x00000001,
        COFF_FILE_HEADER = 0x00000002,

        FULL_READ = 0xFFFFFFFF,
    }
}
