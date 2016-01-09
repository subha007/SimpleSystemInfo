using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Interface
{
    public interface ILayoutModel<TLayoutType> where TLayoutType : struct
    {
        /// <summary>
        /// The main data of the file
        /// </summary>
        TLayoutType Data { get; set; }

        /// <summary>
        /// Get the size of the data
        /// </summary>
        int DataSize { get; }
    }
}
