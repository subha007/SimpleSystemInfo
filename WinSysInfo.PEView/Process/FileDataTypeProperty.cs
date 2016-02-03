using WinSysInfo.PEView.Helper;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// This class is used to determine or manipulate type of file data
    /// to use.
    /// </summary>
    public class FileDataTypeProperty
    {
        #region Properties

        /// <summary>
        /// Defines the type of file data which finally helps in identifying
        /// the type of file reader to use
        /// </summary>
        public EnumFileDataType FileDataType { get; set; }

        /// <summary>
        /// This flag forces to determine the type of file from extension.
        /// Default is true.
        /// </summary>
        public bool UseFileExtensionToDetermineFileDataType { get; set; }

        /// <summary>
        /// The file extension
        /// </summary>
        public string FileExtension { get; protected set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <remarks>
        /// Sets the <see cref="FileDataType"/> to <see cref="EnumFileDataType.BINARY"/>
        /// Sets the <see cref="UseFileExtensionToDetermineFileDataType"/> to <see cref="false"/>
        /// </remarks>
        public FileDataTypeProperty() : this(EnumFileDataType.BINARY, false) { }

        /// <summary>
        /// Constructor to set the file data type
        /// </summary>
        /// <param name="fileDataType">The type of file data</param>
        /// <remarks>
        /// Sets the <see cref="UseFileExtensionToDetermineFileDataType"/> to <see cref="false"/>
        /// </remarks>
        public FileDataTypeProperty(EnumFileDataType fileDataType) : this(fileDataType, false) { }

        /// <summary>
        /// Constructor using only file extesnion to determine type of file data
        /// </summary>
        /// <param name="fileExtn"></param>
        public FileDataTypeProperty(string fileExtn) : this(EnumFileDataType.NONE, true, fileExtn) { }

        /// <summary>
        /// Constructor with all options
        /// </summary>
        /// <param name="fileDataType">The type of file data</param>
        /// <param name="bUseFileExtn">To use file extension or not to use</param>
        /// <param name="fileExtn">
        /// If <see cref="bUseFileExtn"/> is <see cref="true"/> only then this parameter is
        /// considered to determine the type of file data
        /// </param>
        public FileDataTypeProperty(EnumFileDataType fileDataType, bool bUseFileExtn, string fileExtn = StringExHelper.Empty)
        {
            this.FileDataType = fileDataType;
            this.UseFileExtensionToDetermineFileDataType = bUseFileExtn;
            this.FileExtension = fileExtn;

            Rationalize();
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        /// Rationalize the data using the fields.
        /// Set the file data type from extension
        /// </summary>
        public void Rationalize()
        {
            if (!this.UseFileExtensionToDetermineFileDataType) return;

            if (StringExHelper.IsNullOrEmptyOrWhiteSpace(this.FileExtension))
                throw new System.MissingFieldException("File extension must be defined");

            switch (this.FileExtension)
            {
                case ".txt":
                    this.FileDataType = EnumFileDataType.TEXT;
                    break;

                case ".exe":
                    this.FileDataType = EnumFileDataType.BINARY;
                    break;

                case ".xml":
                    this.FileDataType = EnumFileDataType.XML;
                    break;

                default:
                    this.FileDataType = EnumFileDataType.NONE;
                    break;
            }
        }

        #endregion Methods
    }
}
