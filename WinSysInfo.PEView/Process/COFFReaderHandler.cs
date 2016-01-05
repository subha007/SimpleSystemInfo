using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    public abstract class COFFReaderHandler : ICORCOFFReaderHandler
    {
        /// <summary>
        /// Implements the successor link. If null then start of node.
        /// </summary>
        public COFFReaderHandler Successor { get; set; }

        /// <summary>
        /// The type of layout
        /// </summary>
        public EnumReaderLayoutType LayoutType { get; protected set; }

        /// <summary>
        /// Basic Constructor
        /// </summary>
        /// <param name="layoutType"></param>
        public COFFReaderHandler(EnumReaderLayoutType layoutType)
        {
            this.LayoutType = layoutType;
        }

        /// <summary>
        /// Implement the successor data
        /// </summary>
        /// <param name="successor"></param>
        public void SetSuccessor(COFFReaderHandler successor)
        {
            this.Successor = successor;
        }

        /// <summary>
        /// Read the section of file
        /// </summary>
        public virtual void Read()
        {

        }
    }
}
