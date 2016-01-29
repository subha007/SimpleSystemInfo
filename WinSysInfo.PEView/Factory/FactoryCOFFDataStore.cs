using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Factory
{
    public static class FactoryCOFFDataStore
    {
        public static ICOFFDataStore Default()
        {
            return new COFFDataStore();
        }
    }
}
