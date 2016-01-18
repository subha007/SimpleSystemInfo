using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

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
        /// Get or set the reader strategy
        /// </summary>
        public IFileReadStrategy ReaderStrategy { get; set; }

        /// <summary>
        /// The basic constructor to initialize an object file in virtual memory
        /// </summary>
        /// <param name="fullPhysicalPath"></param>
        public ObjectFileReader(string fullPhysicalPath)
        {
            FileInfo fInfo = new FileInfo(fullPhysicalPath);
            if (fInfo.Exists == false)
            {
                throw new FileNotFoundException(string.Format("{0} not found.", fullPhysicalPath));
            }

            this.MemoryFile = MemoryMappedFile.CreateFromFile(fInfo.FullName, FileMode.Open, fInfo.Name);
        }

        /// <summary>
        /// Creates random access to a section of the file
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public void CreateRandomAccess(long offset, long size)
        {
            // If previous accessor is still open close and clean it
            this.CloseReader();

            if(this.MemoryFile != null)
                this.ReaderStrategy = new MemoryRandomAccess(this.MemoryFile, offset, size);
        }

        /// <summary>
        /// Creates random access to a section of the file
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        public void CreateSequentialAccess(long offset, long size)
        {
            // If previous accessor is still open close and clean it
            this.CloseReader();

            if(this.MemoryFile != null)
                this.ReaderStrategy = new MemorySequentialAccess(this.MemoryFile, offset, size);
        }

        /// <summary>
        /// Creates random access to whole file
        /// </summary>
        public void CreateSequentialAccess()
        {
            // If previous accessor is still open close and clean it
            this.CloseReader();

            if (this.MemoryFile != null)
                this.ReaderStrategy = new MemorySequentialAccess(this.MemoryFile);
        }

        /// <summary>
        /// Close the Reader
        /// </summary>
        public void CloseReader()
        {
            // If previous accessor is still open close and clean it
            if (this.ReaderStrategy != null)
            {
                this.ReaderStrategy.Close();
                this.ReaderStrategy = null;
            }
        }

        /// <summary>
        /// Get or set the file offset for which this intermediate reader is created
        /// </summary>
        public long FileOffset
        {
            get { return this.ReaderStrategy.FileOffset; }
        }

        #region Peek

        /// <summary>
        /// Peek ahead bytes but do not chnage the seek pointer in sequential access
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <param name="count">The number of bytes to read. Default is 1.</param>
        /// <returns>A byte array</returns>
        public byte[] PeekBytes(int count = 1, long position = 0)
        {
            return this.ReaderStrategy.PeekBytes(count, position);
        }

        #endregion

        #region Reader

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">Layout Model Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0.</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public LayoutModel<TLayoutType> ReadLayout<TLayoutType>(long position = 0)
            where TLayoutType : struct
        {
            return this.ReaderStrategy.ReadLayout<TLayoutType>(position);
        }

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">Layout Model Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <param name="model"></param>
        /// <returns></returns>
        public void ReadLayout<TLayoutType>(LayoutModel<TLayoutType> model, long position = 0)
            where TLayoutType : struct
        {
            this.ReaderStrategy.ReadLayout<TLayoutType>(model, position);
        }

        /// <summary>
        /// Read bytes
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <param name="count">The number of bytes to read. Default is 1.</param>
        /// <returns></returns>
        public byte[] ReadBytes(int count = 1, int position = 0)
        {
            return this.ReaderStrategy.ReadBytes(position, count);
        }

        /// <summary>
        /// Read boolean data
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public bool? ReadBoolean(long position = 0)
        {
            return this.ReaderStrategy.ReadBoolean(position);
        }

        /// <summary>
        /// Reads a byte value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public byte? ReadByte(long position = 0)
        {
            return this.ReaderStrategy.ReadByte(position);
        }

        /// <summary>
        /// Reads a character from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public char? ReadChar(long position = 0)
        {
            return this.ReaderStrategy.ReadChar(position);
        }

        /// <summary>
        /// Reads a decimal value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public decimal? ReadDecimal(long position = 0)
        {
            return this.ReaderStrategy.ReadDecimal(position);
        }

        /// <summary>
        /// Reads a double-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public double? ReadDouble(long position = 0)
        {
            return this.ReaderStrategy.ReadDouble(position);
        }

        /// <summary>
        /// Reads a 16-bit integer from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public short? ReadInt16(long position = 0)
        {
            return this.ReaderStrategy.ReadInt16(position);
        }

        /// <summary>
        /// Reads a 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public int? ReadInt32(long position = 0)
        {
            return this.ReaderStrategy.ReadInt32(position);
        }

        /// <summary>
        /// Reads a 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public long? ReadInt64(long position = 0)
        {
            return this.ReaderStrategy.ReadInt64(position);
        }

        /// <summary>
        /// Reads an 8-bit signed integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public sbyte? ReadSByte(long position = 0)
        {
            return this.ReaderStrategy.ReadSByte(position);
        }

        /// <summary>
        /// Reads a single-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public float? ReadSingle(long position = 0)
        {
            return this.ReaderStrategy.ReadSingle(position);
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public ushort? ReadUInt16(long position = 0)
        {
            return this.ReaderStrategy.ReadUInt16(position);
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public uint? ReadUInt32(long position = 0)
        {
            return this.ReaderStrategy.ReadUInt32(position);
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns></returns>
        public ulong? ReadUInt64(long position = 0)
        {
            return this.ReaderStrategy.ReadUInt64(position);
        }

        #endregion

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
                if(this.MemoryFile != null)
                {
                    this.MemoryFile.Dispose();
                    this.MemoryFile = null;
                }

                this.CloseReader();
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
