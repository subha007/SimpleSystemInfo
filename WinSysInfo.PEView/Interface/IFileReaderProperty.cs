using WinSysInfo.PEView.Helper;
using WinSysInfo.PEView.Model;
using WinSysInfo.PEView.Process;

namespace WinSysInfo.PEView.Interface
{
    /// <summary>
    /// An interface used to define the set of properties used to parse a file
    /// </summary>
    public interface IFileReaderProperty
    {
        /// <summary>
        /// Get or Set the file data property
        /// </summary>
        FileDataTypeProperty FileDataType { get; set; }

        /// <summary>
        /// Get or set the file path
        /// </summary>
        HugeFilePathHelper FilePath { get; set; }

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
