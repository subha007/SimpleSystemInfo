using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The class to represent Stub
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack=1)]
    public struct MSDOSStubLayout
    {
        /// <summary>
        /// A Stub which generally contains a message and
        /// a assembly code section to execute
        /// </summary>
        public byte[] Stub;
    }
}
