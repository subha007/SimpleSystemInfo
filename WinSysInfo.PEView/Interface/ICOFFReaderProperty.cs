using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Interface
{
    public interface ICOFFReaderProperty
    {
        /// <summary>
        /// Specifies the data structures to read
        /// </summary>
        EnumCOFFReaderType ReaderType { get; set; }

        /// <summary>
        /// Full COFF file path
        /// </summary>
        string FullFilePath { get; }

        /// <summary>
        /// GUID 
        /// </summary>
        string UniqueFileName { get; }

        /// <summary>
        /// The offset in the file from which to create file reader
        /// </summary>
        long OffsetOfFile { get; }

        /// <summary>
        /// The size of file from offset to create a reader
        /// </summary>
        long SizeOfReader { get; }

        /// <summary>
        /// Validate the data
        /// </summary>
        /// <returns></returns>
        bool TryValidate();

        /// <summary>
        /// Any cleanup job
        /// </summary>
        void Cleanup();
    }
}
