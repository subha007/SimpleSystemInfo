using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Factory
{
    public static class FactoryCOFFDataStore
    {
        public static ICOFFDataStore Default()
        {
            return new COFFDataStore();
        }

        public static ICOFFDataStore PEStore()
        {
            return new PEFileLayout();
        }
    }
}
