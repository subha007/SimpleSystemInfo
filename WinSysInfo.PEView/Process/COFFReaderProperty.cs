using System.IO;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    public class COFFReaderProperty : ICOFFReaderProperty
    {
        /// <summary>
        /// Specifies the data structures to read
        /// </summary>
        public EnumCOFFReaderType ReaderType { get; set; }

        /// <summary>
        /// Full COFF file path
        /// </summary>
        public string FullFilePath
        {
            get
            {
                return ((this.FileBuffer != null)? this.FileBuffer.GetFile() : string.Empty);
            }
        }

        /// <summary>
        /// GUID 
        /// </summary>
        public string UniqueFileName
        {
            get { return this.FileBuffer.GetFileNameUnique(); }
        }

        /// <summary>
        /// The offset in the file from which to create file reader
        /// </summary>
        public long OffsetOfFile { get; protected set; }

        /// <summary>
        /// The size of file from offset to create a reader
        /// </summary>
        public long SizeOfReader { get; protected set; }

        /// <summary>
        /// Actual File decision based on Windows OS
        /// </summary>
        protected TempFileBuffer FileBuffer { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public COFFReaderProperty() 
            : this(string.Empty
            ,EnumCOFFReaderType.MEMORY_SEQ_READ
            ,0
            ,0)
        { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public COFFReaderProperty(string fullFilePath) 
            : this(fullFilePath
            ,EnumCOFFReaderType.MEMORY_SEQ_READ
            ,0
            ,0) 
        { }

        /// <summary>
        /// Default constructor
        /// </summary>
        public COFFReaderProperty(string fullFilePath
            ,EnumCOFFReaderType readerType
            ,long offset
            ,long size)
        {
            this.FileBuffer = new TempFileBuffer(fullFilePath);
            this.ReaderType = readerType;
            this.OffsetOfFile = offset;
            this.SizeOfReader = size;
        }

        /// <summary>
        /// Validate the data
        /// </summary>
        /// <returns></returns>
        public bool TryValidate()
        {
            if (File.Exists(this.FullFilePath) == false)
                return false;

            this.FileBuffer.Rationalize();

            return true;
        }

        /// <summary>
        /// Any cleanup job
        /// </summary>
        public void Cleanup()
        {
            this.FileBuffer.Cleanup();
        }
    }
}
