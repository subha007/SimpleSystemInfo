using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Interface
{
    internal interface ICOFFFileLayoutBrowse
    {
        void Read(IFileReadStrategy readStrategy);
    }
}
