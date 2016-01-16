using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.UnitTest
{
    [TestClass]
    public class PETestReader
    {
        [TestMethod]
        public void TestReadPE()
        {
            COFFBrowserBase browser = new COFFBrowserBase(@"C:\Users\user\Downloads\PEview\PEview.exe");
            browser.Read();
        }
    }
}
