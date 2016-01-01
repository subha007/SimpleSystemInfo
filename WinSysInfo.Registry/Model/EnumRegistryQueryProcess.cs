using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventryWinReg.Process
{
    /// <summary>
    /// This enumeration provides different ways to process and read the window registry
    /// using the class <see cref="ProcessRegistryQuery"/>.
    /// The values are used as flags and hence can be used in combination but remeber not 
    /// all combinations are allowed.
    /// </summary>
    [Flags]
    public enum EnumRegistryQueryProcess
    {
        NONE = 0x00000001,
        ADD_ALL_SUBKEYS = 0x00000002,
        ADD_ALL_VALUES = 0x00000004,
    }
}
