using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    public class MSDOSHeaderReaderHandler : COFFReaderHandler
    {
        /// <summary>
        /// The actual data
        /// </summary>
        public MSDOSHeaderLayout Data { get; set; }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="layoutType"></param>
        public MSDOSHeaderReaderHandler(EnumReaderLayoutType layoutType)
            : base(layoutType)
        {
        }
    }
}
