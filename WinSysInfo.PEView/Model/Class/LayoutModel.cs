using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The main layout class model which wraps the layout raw structure
    /// </summary>
    /// <typeparam name="TLayoutType"></typeparam>
    public class LayoutModel<TLayoutType> where TLayoutType : struct
    {
        /// <summary>
        /// Get or set the layout type
        /// </summary>
        public EnumReaderLayoutType LayoutType { get; set; }

        /// <summary>
        /// Get or set the main data object
        /// </summary>
        public TLayoutType Data { get; set; }

        /// <summary>
        /// Get the size of the data
        /// </summary>
        public static int DataSize
        {
            get
            {
                return Marshal.SizeOf(typeof(TLayoutType));
            }
        }
    }
}
