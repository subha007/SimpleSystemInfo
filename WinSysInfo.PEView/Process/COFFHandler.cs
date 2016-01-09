using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    public abstract class COFFHandler<TLayoutType> : ILayoutHandler<TLayoutType> where TLayoutType : struct
    {
        /// <summary>
        /// Implements the successor link. If null then start of node.
        /// </summary>
        public LayoutModel<TLayoutType> Model { get; set; }

        /// <summary>
        /// Implements the successor link. If null then start of node.
        /// </summary>
        public COFFHandler<TLayoutType> NextHandler { get; set; }

        /// <summary>
        /// Implements the previous link
        /// </summary>
        public COFFHandler<TLayoutType> PrevHandler { get; protected set; }

        /// <summary>
        /// Use of internal Reader wrapper
        /// </summary>
        private ObjectFileReader Reader { get; protected set; }

        /// <summary>
        /// Constructor to initialize with object reader and previous handler
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="prevhandler"></param>
        public COFFHandler(ObjectFileReader reader, COFFHandler<TLayoutType> prevhandler)
        {
            this.Model = new LayoutModel<TLayoutType>();
            this.PrevHandler = prevhandler;
            this.Reader = reader;
        }

        /// <summary>
        /// Implement the successor data
        /// </summary>
        /// <param name="successor"></param>
        public void SetSuccessor(COFFHandler<TLayoutType> successor)
        {
            this.NextHandler = successor;
        }
    }
}
