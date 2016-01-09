using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// Defines an interface for handling the requests
    /// </summary>
    public class COFFHeaderReaderHandler : ILayoutRead<TLayoutType> where TLayoutType : struct
    {
        /// <summary>
        /// The actual data
        /// </summary>
        public COFFFileHeader Data { get; set; }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="layoutType"></param>
        public COFFHeaderReaderHandler(EnumReaderLayoutType layoutType)
            : base(layoutType)
        {
        }
    }
}
