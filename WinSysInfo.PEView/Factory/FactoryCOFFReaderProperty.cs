using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Factory
{
    public static class FactoryCOFFReaderProperty
    {
        public static IFileReaderProperty Default()
        {
            return new COFFReaderProperty();
        }

        public static IFileReaderProperty New(string fullFilePath
                                            , EnumCOFFReaderType readerType
                                            , long offset
                                            , long size)
        {
            return new COFFReaderProperty(fullFilePath, readerType, offset, size);
        }
    }
}
