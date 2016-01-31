using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Factory
{
    public static class FactoryCOFFFileBrowse
    {
        public static ICOFFFileLayoutBrowse Browser(EnumCOFFFileType fileType, IFileReadStrategy readerStrategy)
        {
            switch(fileType)
            {
                case EnumCOFFFileType.PE:
                    return new PEFileLayoutBrowse(readerStrategy);
            }

            return null;
        }
    }
}
