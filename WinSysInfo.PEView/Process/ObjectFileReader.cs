using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// An internal class to read binary, object files
    /// </summary>
    public class ObjectFileReader : IDisposable
    {
        /// <summary>
        /// In .NET 4 + this is the best method
        /// </summary>
        private MemoryMappedFile MemoryFile { get; set; }

        /// <summary>
        /// Represents a randomly accessed view of a memory-mapped file.
        /// </summary>
        protected MemoryMappedViewAccessor Accessor { get; set; }

        /// <summary>
        /// Use the stream returned by this method for sequential access to a memory-mapped file, 
        /// such as for inter-process communications.
        /// </summary>
        protected MemoryMappedViewStream IoAccess { get; set; }

        /// <summary>
        /// The basic constructor to initialize an object file in virtual memory
        /// </summary>
        /// <param name="fullPhysicalPath"></param>
        internal ObjectFileReader(string fullPhysicalPath)
        {
            FileInfo fInfo = new FileInfo(fullPhysicalPath);
            if(fInfo.Exists == false)
            {
                throw new FileNotFoundException(string.Format("{0} not found.", fullPhysicalPath));
            }

            this.MemoryFile = MemoryMappedFile.CreateFromFile(fInfo.FullName, FileMode.Open, fInfo.Name);
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~ObjectFileReader()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose wrapper. Actual dispose method.
        /// </summary>
        /// <param name="itIsSafeToAlsoFreeManagedObjects"></param>
        protected void Dispose(Boolean itIsSafeToAlsoFreeManagedObjects)
        {
            // Free unmanaged resources

            // Free managed resources if applicable
            if(itIsSafeToAlsoFreeManagedObjects == true)
            {
                if (this.Accessor != null)
                {
                    this.Accessor.Dispose();
                    this.Accessor = null;
                }

                if(this.IoAccess != null)
                {
                    this.IoAccess.Close();
                    this.IoAccess.Dispose();
                    this.IoAccess = null;
                }

                if(this.MemoryFile != null)
                {
                    this.MemoryFile.Dispose();
                    this.MemoryFile = null;
                }
            }
        }

        /// <summary>
        /// Interface implementation
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
