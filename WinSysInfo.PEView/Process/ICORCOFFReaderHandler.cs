using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Process
{
    interface ICORCOFFReaderHandler
    {
        void SetSuccessor(COFFReaderHandler successor);
    }
}
