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
        {
            this.MemoryFile = memoryFile;
            this.IsOpen = false;
        }

        /// <summary>
        /// Basic constructor
        /// </summary>
        /// <param name="memoryFile"></param>
        public MemorySequentialAccess(MemoryMappedFile memoryFile, long offset, long size)
        {
            this.MemoryFile = memoryFile;
            this.IsOpen = false;
            if(this.MemoryFile != null)
            {
                this.IoAccess = this.MemoryFile.CreateViewStream(offset, size);
                this.IsOpen = true;
            }
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
                this.IoAccess = this.MemoryFile.CreateViewStream(offset, size);

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
            if(this.IoAccess != null)
            {
                byte[] bytes = this.ReadBytes(0, LayoutModel<TLayoutType>.DataSize);

                GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                model.Data = (TLayoutType) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TLayoutType));
                handle.Free();
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
            if(this.IoAccess != null)
            {
                byte[] bytes = this.ReadBytes(0, LayoutModel<TLayoutType>.DataSize);

                GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
                model.Data = (TLayoutType) Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(TLayoutType));
                handle.Free();
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
            if(this.IoAccess != null && count > 0)
            {
                byte[] iodata = new byte[1];
                this.IoAccess.Read(iodata, (int) 0, count);
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
            byte[] iodata = ReadBytes(position, 1);
            if(iodata != null)
                return BitConverter.ToBoolean(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a byte value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public byte? ReadByte(long position)
        {
            byte[] iodata = ReadBytes(position, 1);
            if(iodata != null)
                return iodata[0];

            return null;
        }

        /// <summary>
        /// Reads a character from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public char? ReadChar(long position)
        {
            byte[] iodata = ReadBytes(position, 1);
            if(iodata != null)
                return BitConverter.ToChar(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a decimal value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public decimal? ReadDecimal(long position)
        {
            byte[] iodata = ReadBytes(position, 16);
            if(iodata != null)
                return BitConverterExtended.ToDecimal(iodata);

            return null;
        }

        /// <summary>
        /// Reads a double-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public double? ReadDouble(long position)
        {
            byte[] iodata = ReadBytes(position, 8);
            if(iodata != null)
                return BitConverter.ToDouble(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a 16-bit integer from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public short? ReadInt16(long position)
        {
            byte[] iodata = ReadBytes(position, 2);
            if(iodata != null)
                return BitConverter.ToInt16(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public int? ReadInt32(long position)
        {
            byte[] iodata = ReadBytes(position, 4);
            if(iodata != null)
                return BitConverter.ToInt32(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads a 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public long? ReadInt64(long position)
        {
            byte[] iodata = ReadBytes(position, 8);
            if(iodata != null)
                return BitConverter.ToInt64(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an 8-bit signed integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public sbyte? ReadSByte(long position)
        {
            byte[] iodata = ReadBytes(position, 1);
            if(iodata != null)
                return Convert.ToSByte(iodata[0]);

            return null;
        }

        /// <summary>
        /// Reads a single-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public float? ReadSingle(long position)
        {
            byte[] iodata = ReadBytes(position, 4);
            if(iodata != null)
                return BitConverter.ToSingle(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an unsigned 16-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public ushort? ReadUInt16(long position)
        {
            byte[] iodata = ReadBytes(position, 2);
            if(iodata != null)
                return BitConverter.ToUInt16(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an unsigned 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public uint? ReadUInt32(long position)
        {
            byte[] iodata = ReadBytes(position, 4);
            if(iodata != null)
                return BitConverter.ToUInt32(iodata, 0);

            return null;
        }

        /// <summary>
        /// Reads an unsigned 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        public ulong? ReadUInt64(long position)
        {
            byte[] iodata = ReadBytes(position, 4);
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
