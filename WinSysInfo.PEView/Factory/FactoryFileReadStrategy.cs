using System;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Factory
{
    public static class FactoryFileReadStrategy
    {
        public static IFileReadStrategy Instance(IFileReaderProperty readerProperty)
        {
            IFileReadStrategy readStrategy = null;
            switch(readerProperty.ReaderType)
            {
                case EnumCOFFReaderType.MEMORY_SEQ_READ:
                    readStrategy = new MemorySequentialAccess(readerProperty);
                    break;

                case EnumCOFFReaderType.MEMORY_ACCESSOR_READ:
                    readStrategy = new MemoryRandomAccess(readerProperty);
                    break;

                case EnumCOFFReaderType.BINARY_READ:
                    break;

                default:
                    throw new NotImplementedException("Unreachable code");
            }

            return readStrategy;
        }
    }
}
