using System;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Interface;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Process
{
    /// <summary>
    /// Represents a randomly accessed view of a memory-mapped file
    /// </summary>
    public class MemoryRandomAccess : IFileReadStrategy, IDisposable
    {
        #region Main Properties

        /// <summary>
        /// In .NET 4 + this is the best method
        /// </summary>
        private MemoryMappedFile MemoryFile { get; set; }

        /// <summary>
        /// Represents a randomly accessed view of a memory-mapped file.
        /// </summary>
        protected MemoryMappedViewAccessor Accessor { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="memoryFile"></param>
        public MemoryRandomAccess(MemoryMappedFile memoryFile)
        {
            this.MemoryFile = memoryFile;
            this.IsOpen = false;
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="memoryFile"></param>
        public MemoryRandomAccess(MemoryMappedFile memoryFile, long offset, long size)
        {
            this.MemoryFile = memoryFile;
            this.IsOpen = false;
            if(this.MemoryFile != null)
            {
                this.Accessor = this.MemoryFile.CreateViewAccessor(offset, size);
                this.IsOpen = true;
            }
        }

        #endregion

        #region OpenClose

        /// <summary>
        /// Get or set random access is open
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Open the random accessor
        /// </summary>
        /// <param name="offset">The offset in the file from which to create file reader</param>
        /// <param name="size">The size of file from offset to create a reader</param>
        public void Open(long offset, long size)
        {
            if(this.IsOpen == false)
                this.Accessor = this.MemoryFile.CreateViewAccessor(offset, size);

            this.IsOpen = true;
        }

        /// <summary>
        /// Close random access
        /// </summary>
        public void Close()
        {
            if(this.Accessor != null)
            {
                this.Accessor.Dispose();
                this.Accessor = null;
            }
        }

        #endregion

        #region Reader

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">The Layout Model value Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The structure to contain the read data</returns>
        public LayoutModel<TLayoutType> ReadLayout<TLayoutType>(int position)
            where TLayoutType : struct
        {
            LayoutModel<TLayoutType> model = new LayoutModel<TLayoutType>();
            if(this.Accessor != null)
            {
                TLayoutType fileData;
                this.Accessor.Read<TLayoutType>(position, out fileData);
                model.Data = fileData;
            }

            return model;
        }

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">The Layout Model value Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <param name="model">The structure to contain the read data</param>
        public void ReadLayout<TLayoutType>(int position, LayoutModel<TLayoutType> model)
            where TLayoutType : struct
        {
            if(this.Accessor != null)
            {
                TLayoutType fileData;
                this.Accessor.Read<TLayoutType>(position, out fileData);
                model.Data = fileData;
            }
        }

        /// <summary>
        /// Read bytes
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>A byte array</returns>
        public byte[] ReadBytes(long position, int count)
        {
            if(this.Accessor != null && count > 0)
            {
                byte[] iodata = new byte[1];
                this.Accessor.ReadArray(position, iodata, (int) 0, count);
                return iodata;
            }

            return null;
        }

        /// <summary>
        /// Read boolean data
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public bool? ReadBoolean(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadBoolean(position);
            return null;
        }

        /// <summary>
        /// Reads a byte value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public byte? ReadByte(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadByte(position);
            return null;
        }

        /// <summary>
        /// Reads a character from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public char? ReadChar(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadChar(position);
            return null;
        }

        /// <summary>
        /// Reads a decimal value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public decimal? ReadDecimal(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadDecimal(position);
            return null;
        }

        /// <summary>
        /// Reads a double-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public double? ReadDouble(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadDouble(position);
            return null;
        }

        /// <summary>
        /// Reads a 16-bit integer from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public short? ReadInt16(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadInt16(position);
            return null;
        }

        /// <summary>
        /// Reads a 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public int? ReadInt32(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadInt32(position);
            return null;
        }

        /// <summary>
        /// Reads a 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public long? ReadInt64(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadInt64(position);
            return null;
        }

        /// <summary>
        /// Reads an 8-bit signed integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public sbyte? ReadSByte(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadSByte(position);
            return null;
        }

        /// <summary>
        /// Reads a single-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public float? ReadSingle(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadSingle(position);
            return null;
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public ushort? ReadUInt16(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadUInt16(position);
            return null;
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public uint? ReadUInt32(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadUInt32(position);
            return null;
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public ulong? ReadUInt64(long position)
        {
            if(this.Accessor != null)
                return this.Accessor.ReadUInt64(position);
            return null;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Destructor
        /// </summary>
        ~MemoryRandomAccess()
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
