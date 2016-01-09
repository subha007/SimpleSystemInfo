using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Interface
{
    public interface ILayoutRead<TLayoutType>
    {
        /// <summary>
        /// The main data of the file
        /// </summary>
        TLayoutType Data { get; set; }

        void Read();
    }
}
