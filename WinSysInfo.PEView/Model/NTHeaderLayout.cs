using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// After the MSDOS stub, at the file offset specified at offset 0x3c,
    /// is a 4-byte signature that identifies the file as a PE format image file
    /// </summary>
    public class NTHeaderLayout
    {
        /// <summary>
        /// This signature is “PE\0\0” (the letters “P” and “E” followed by two 
        /// null bytes).
        /// </summary>
        private char[] signature = new char[4];
        public char[] Signature
        {
            get { return signature; }
            set { signature = value; }
        }
    }
}
