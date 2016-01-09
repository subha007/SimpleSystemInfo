using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Interface
{
    /// <summary>
    /// The class which mainly handles the reading of files
    /// </summary>
    /// <typeparam name="TLayoutType"></typeparam>
    public interface ILayoutHandler<TLayoutType> where TLayoutType : struct
    {
        ILayoutModel<TLayoutType> Model { get; set; }
        ILayoutHandler<TLayoutType> PrevHandler { get; set; }
        ILayoutHandler<TLayoutType> NextHandler { get; set; }
        ILayoutRead<TLayoutType> ReadAction { get; set; }
    }
}
