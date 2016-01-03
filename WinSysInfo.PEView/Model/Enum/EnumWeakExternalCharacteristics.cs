using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// Auxiliary Format 3: Weak Externals Charcateristics
    /// </summary>
    public enum EnumWeakExternalCharacteristics
    {
        IMAGE_WEAK_EXTERN_SEARCH_NOLIBRARY = 1,
        IMAGE_WEAK_EXTERN_SEARCH_LIBRARY = 2,
        IMAGE_WEAK_EXTERN_SEARCH_ALIAS = 3
    }
}
