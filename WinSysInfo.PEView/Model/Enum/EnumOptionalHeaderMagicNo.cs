using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The optional header magic number determines whether an 
    /// image is a PE32 or PE32+ executable.
    /// </summary>
    public enum EnumOptionalHeaderMagicNo : ushort
    {
        PE32 = 0x10b,
        PE32Plus = 0x20b,
        ROMIMage = 0x107
    }
}
