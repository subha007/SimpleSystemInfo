using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinSysInfo.PEView.Model;

namespace WinSysInfo.PEView.Interface
{
    /// <summary>
    /// This interface defines a strategy to read a file.
    /// </summary>
    public interface IFileReadStrategy
    {
        #region OpenClose

        /// <summary>
        /// Open the random accessor
        /// </summary>
        /// <param name="offset">The offset in the file from which to create file reader</param>
        /// <param name="size">The size of file from offset to create a reader</param>
        void Open(long offset, long size);

        /// <summary>
        /// Close random access
        /// </summary>
        void Close();

        /// <summary>
        /// Get or set if file is open
        /// </summary>
        bool IsOpen { get; set; }

        #endregion

        #region 

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">The Layout Model value Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The structure to contain the read data</returns>
        LayoutModel<TLayoutType> ReadLayout<TLayoutType>(int position)
            where TLayoutType : struct;

        /// <summary>
        /// Read a layout model
        /// </summary>
        /// <typeparam name="T">The Layout Model value Type</typeparam>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <param name="model">The structure to contain the read data</param>
        void ReadLayout<TLayoutType>(int position, LayoutModel<TLayoutType> model)
            where TLayoutType : struct;
        
        /// <summary>
        /// Read bytes
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <param name="count">The number of bytes to read.</param>
        /// <returns>A byte array</returns>
        byte[] ReadBytes(long position, int count);

        /// <summary>
        /// Read boolean data
        /// </summary>
        /// <param name="position"></param>
        /// <returns>The value that was read or null if cannot read.</returns>
        bool? ReadBoolean(long position);

        /// <summary>
        /// Reads a byte value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        byte? ReadByte(long position);

        /// <summary>
        /// Reads a character from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        char? ReadChar(long position);

        /// <summary>
        /// Reads a decimal value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        decimal? ReadDecimal(long position);

        /// <summary>
        /// Reads a double-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        double? ReadDouble(long position);

        /// <summary>
        /// Reads a 16-bit integer from the accessor.
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        short? ReadInt16(long position);

        /// <summary>
        /// Reads a 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        int? ReadInt32(long position);

        /// <summary>
        /// Reads a 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        long? ReadInt64(long position);

        /// <summary>
        /// Reads an 8-bit signed integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        sbyte? ReadSByte(long position);

        /// <summary>
        /// Reads a single-precision floating-point value from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        float? ReadSingle(long position);

        /// <summary>
        /// Reads an unsigned 16-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        ushort? ReadUInt16(long position);

        /// <summary>
        /// Reads an unsigned 32-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        uint? ReadUInt32(long position);

        /// <summary>
        /// Reads an unsigned 64-bit integer from the accessor
        /// </summary>
        /// <param name="position">The position in the file at which to begin reading</param>
        /// <returns>The value that was read or null if cannot read.</returns>
        ulong? ReadUInt64(long position);

        #endregion
    }
}