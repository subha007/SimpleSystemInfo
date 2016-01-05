using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    public class COFFReaderProperty
    {
        /// <summary>
        /// Specifies the data structures to read
        /// </summary>
        public EnumReaderLayoutType LayoutFlag { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public COFFReaderProperty()
        {
            this.LayoutFlag = EnumReaderLayoutType.FULL_READ;
        }
    }
}
