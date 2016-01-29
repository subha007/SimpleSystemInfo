using System.IO;
using System.Text.RegularExpressions;
namespace WinSysInfo.PEView.Process
{
    public class TempFileBuffer
    {
        /// <summary>
        /// On Windows-based platforms, paths must be less than 248 characters, and 
        /// file names must be less than 260 characters
        /// </summary>
        internal static readonly uint MaxPath = 259;

        public bool UsingTempFile { get; protected set; }
        public string TempFile { get; set; }
        public string ActualFile { get; set; }
        public string FileName { get; protected set; }

        public TempFileBuffer() : this(string.Empty) { }

        public TempFileBuffer(string actualfile)
        {
            this.ActualFile = actualfile;
            this.TempFile = string.Empty;
            this.UsingTempFile = false;
        }

        public void Rationalize()
        {
            if(string.IsNullOrEmpty(this.ActualFile) == true ||
                (this.ActualFile.Length > TempFileBuffer.MaxPath))
            {
                // Create Temp file and use
                this.TempFile = System.Native.IO.FileSystem.Path.GetTempFileName();
                this.UsingTempFile = true;
            }

            if(string.IsNullOrEmpty(this.ActualFile) == false &&
                (this.ActualFile.Length > TempFileBuffer.MaxPath))
            {
                System.Native.IO.FileSystem.File.Copy(this.ActualFile, this.TempFile, true);
            }
        }

        public string GetFile()
        {
            if (this.UsingTempFile)
                return this.TempFile;
            else
                return this.ActualFile;
        }

        public string GetFileNameUnique()
        {
            string strFull = this.GetFile();

            if (string.IsNullOrEmpty(strFull) == true)
                strFull = System.Guid.NewGuid().ToString();
            else
            {
                strFull = Regex.Replace(strFull, @"\\|:|/|\.", string.Empty);
            }

            return strFull;
        }

        /// <summary>
        /// Any cleanup job
        /// </summary>
        public void Cleanup()
        {
            if (this.UsingTempFile)
            {
                System.Native.IO.FileSystem.File.Delete(this.TempFile, true);
                this.TempFile = string.Empty;
                this.UsingTempFile = false;
            }
        }
    }
}
