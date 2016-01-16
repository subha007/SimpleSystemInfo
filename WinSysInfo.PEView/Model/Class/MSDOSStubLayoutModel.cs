using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// Wrapper of structure MSDOSStubLayout
    /// </summary>
    public class MSDOSStubLayoutModel : LayoutModel<MSDOSStubLayout>
    {
        /// <summary>
        /// Set data
        /// </summary>
        /// <param name="byteData"></param>
        public void SetData(byte[] byteData)
        {
            this.actualData.Stub = byteData;
        }
    }
}
