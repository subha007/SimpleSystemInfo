using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// PE32 contains this additional field, which is absent in PE32+, 
    /// following BaseOfCode.
    /// </summary>
    public class OptionalHeaderStandardFields32 : OptionalHeaderStandardFields
    {
        /// <summary>
        /// The address that is relative to the image base of the beginning-of-data
        /// section when it is loaded into memory
        /// </summary>
        public uint BaseOfData { get; set; }
    }
}
