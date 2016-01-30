using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    internal class PEFileLayoutBrowse
    {
        internal static bool HasPEHeader(IFileReadStrategy ReaderStrategy)
        {
            // Check if this is a PE/COFF file.
            char[] sigchars = System.Text.Encoding.UTF8.GetString(
                                    ReaderStrategy.PeekBytes(2, -1))
                                    .ToCharArray();

            return sigchars.SequenceEqual(ConstantWinCOFFImage.MSDOSMagic);
        }
    }
}
