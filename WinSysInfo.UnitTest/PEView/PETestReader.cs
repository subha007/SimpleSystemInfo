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
            COFFReaderProperty readProperty = new COFFReaderProperty(@"C:\TEMP\PEview\WinSysInfoTest.exe");
            COFFBrowserBase browser = new COFFBrowserBase(readProperty);
            browser.Read();
        }
    }
}
