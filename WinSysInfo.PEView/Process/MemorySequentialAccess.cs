using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    public class MemorySequentialAccess : IFileReadStrategy, IDisposable
    {
        #region Main Properties

        /// <summary>
        /// In .NET 4 + this is the best method
        /// </summary>
        private MemoryMappedFile MemoryFile { get; set; }

        /// <summary>
        /// Use the stream returned by this method for sequential access to a memory-mapped file, 
        /// such as for inter-process communications.
        /// </summary>
        protected MemoryMappedViewStream IoAccess { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="memoryFile"></param>
        public MemorySequentialAccess(MemoryMappedFile memoryFile)
            : this(memoryFile, 0, 0)
        {
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="memoryFile"></param>
        public MemorySequentialAccess(MemoryMappedFile memoryFile, long offset, long size)
        {
            this.MemoryFile = memoryFile;
            if(this.MemoryFile != null)
            {
                this.IoAccess = this.MemoryFile.CreateViewStream(offset, size, MemoryMappedFileAccess.Read);
                this.IsOpen = true;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// Set file offset
        /// </summary>
        /// <param name="offset"></param>
        private void SetFileOffset(long offset)
        {
            this.IoAccess.Seek(offset, System.IO.SeekOrigin.Begin);
        }

        #endregion

        #region OpenClose

        /// <summary>
        /// Open the random accessor
        /// </summary>
        /// <param name="offset">The offset in the file from which to create file reader</param>
        /// <param name="size">The size of file from offset to create a reader</param>
        public void Open(long offset, long size)
        {
            if(this.IsOpen == false)
                this.IoAccess = this.MemoryFile.CreateViewStream(offset, size, MemoryMappedFileAccess.Read);

            this.IsOpen = true;
        }

        /// <summary>
        /// Close random access
        /// </summary>
        public void Close()
        {
            if(this.IoAccess != null)
            {
                this.IoAccess.Close();
                this.IoAccess.Dispose();
                this.IoAccess = null;
            }
        }

        /// <summary>
        /// Get or set random access is open
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Get or set the file offset for which this intermediate reader is created
        /// </summary>
        public long FileOffset 
        {
            get { return (this.IoAccess != null) ? this.IoAccess.Position : -1; }
        }

        #endregion

        #region Peek

        /// <summary>
        /// Peek ahead bytes but do not chnage the seek pointer in sequential access
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <param name="count">The number of bytes to read. Default 1.</param>
        /// <returns>A byte array</returns>
        public byte[] PeekBytes(int count = 1, long position = 0)
        {
            using(MemoryMappedViewStream tempPeek = this.MemoryFile.CreateViewStream(position, count, MemoryMappedFileAccess.Read))
            {
                byte[] iodata = new byte[count];
                tempPeek.Read(iodata, (int) 0, count);
                return iodata;
            }
        }

        #endregion

        #region Reader With Position

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">The Layout Model value Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The structure to contain the read data</returns>
        public LayoutModel<TLayoutType> ReadLayout<TLayoutType>(long position = 0)
            where TLayoutType : struct
        {
            LayoutModel<TLayoutType> model = new LayoutModel<TLayoutType>();
            this.ReadLayout<TLayoutType>(model, position);

            return model;
        }

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">The Layout Model value Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <param name="model">The structure to contain the read data</param>
        public void ReadLayout<TLayoutType>(LayoutModel<TLayoutType> model, long position = 0)
            where TLayoutType : struct
        {
            if(this.IoAccess != null)
            {
                // Seek Position from current
                this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

                byte[] bytes = this.ReadBytes(LayoutModel<TLayoutType>.DataSize, position);

                GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                model.SetData(Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TLayoutType)));
                handle.Free();
            }
        }

        /// <summary>
        /// Read bytes
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <param name="count">The number of bytes to read. Default is 1.</param>
        /// <returns>A byte array</returns>
        public byte[] ReadBytes(int count = 1, long position = 0)
        {
            if(this.IoAccess != null && count > 0)
            {
                // Seek Position from current
                this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

                byte[] iodata = new byte[count];
                this.IoAccess.Read(iodata, (int) 0, count);
                return iodata;
            }

            return null;
        }

        /// <summary>
        /// Read boolean data
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public bool? ReadBoolean(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(1, position);
            if(iodata != null)
                return BitConverter.ToBoolean(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a byte value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public byte? ReadByte(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(1, position);
            if(iodata != null)
                return iodata[0];

            return null;
        }

        /// <summary>
        /// Reads a character from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public char? ReadChar(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(1, position);
            if(iodata != null)
                return BitConverter.ToChar(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a decimal value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public decimal? ReadDecimal(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(16, position);
            if(iodata != null)
                return BitConverterExtended.ToDecimal(iodata);

            return null;
        }

        /// <summary>
        /// Reads a double-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public double? ReadDouble(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(8, position);
            if(iodata != null)
                return BitConverter.ToDouble(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a 16-bit integer from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public short? ReadInt16(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(2, position);
            if(iodata != null)
                return BitConverter.ToInt16(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public int? ReadInt32(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(4, position);
            if(iodata != null)
                return BitConverter.ToInt32(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public long? ReadInt64(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(8, position);
            if(iodata != null)
                return BitConverter.ToInt64(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an 8-bit signed integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public sbyte? ReadSByte(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(1, position);
            if(iodata != null)
                return Convert.ToSByte(iodata[0]);

            return null;
        }

        /// <summary>
        /// Reads a single-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public float? ReadSingle(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(4, position);
            if(iodata != null)
                return BitConverter.ToSingle(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public ushort? ReadUInt16(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(2, position);
            if(iodata != null)
                return BitConverter.ToUInt16(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public uint? ReadUInt32(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(4, position);
            if(iodata != null)
                return BitConverter.ToUInt32(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading
        /// relative to the current position in the file. Default is 0</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public ulong? ReadUInt64(long position = 0)
        {
            // Seek Position from current
            this.IoAccess.Seek(position, System.IO.SeekOrigin.Current);

            byte[] iodata = ReadBytes(4, position);
            if(iodata != null)
                return BitConverter.ToUInt64(iodata, 0);

            return null;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Destructor
        /// </summary>
        ~MemorySequentialAccess()
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
                this.Close();
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

        #endregion
    }
}
