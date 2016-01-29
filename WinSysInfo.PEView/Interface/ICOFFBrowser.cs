namespace WinSysInfo.PEView.Interface
{
    /// <summary>
    /// A generic interface to browse through the PE / COFF file
    /// </summary>
    public interface ICOFFBrowser
    {
        #region Properties

        /// <summary>
        /// Get or set the reader strategy
        /// </summary>
        IFileReadStrategy ReaderStrategy { get; }

        /// <summary>
        /// Data mapping
        /// </summary>
        ICOFFDataStore DataStore { get; set; }

        /// <summary>
        /// Reader property
        /// </summary>
        ICOFFReaderProperty ReaderProperty { get; set; }

        #endregion
    }
}
