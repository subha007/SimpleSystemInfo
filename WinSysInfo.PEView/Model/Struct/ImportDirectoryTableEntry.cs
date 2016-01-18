using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The Import Directory Table. There is a single array of these and one entry 
    /// per imported DLL.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ImportDirectoryTableEntry
    {
        public uint ImportLookupTableRVA { get; set; }
        public uint TimeDateStamp { get; set; }
        public uint ForwarderChain { get; set; }
        public uint NameRVA { get; set; }
        public uint ImportAddressTableRVA { get; set; }
    }
}
