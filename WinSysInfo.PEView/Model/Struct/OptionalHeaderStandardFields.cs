﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The first eight fields of the optional header are standard 
    /// fields that are defined for every implementation of COFF. 
    /// These fields contain general information that is useful for 
    /// loading and running an executable file. They are unchanged 
    /// for the PE32+ format
    /// </summary>
    public class OptionalHeaderStandardFields
    {
        /// <summary>
        /// The unsigned integer that identifies the state of the image file.
        /// </summary>
        public EnumOptionalHeaderMagicNo Magic { get; set; }

        /// <summary>
        /// The linker major version number
        /// </summary>
        public byte MajorLinkerVersion { get; set; }

        /// <summary>
        /// The linker minor version number
        /// </summary>
        public byte MinorLinkerVersion { get; set; }

        /// <summary>
        /// The size of the code (text) section, or the sum of all code sections
        /// if there are multiple sections
        /// </summary>
        public uint SizeOfCode { get; set; }

        /// <summary>
        /// The size of the initialized data section, or the sum of all such 
        /// sections if there are multiple data sections
        /// </summary>
        public uint SizeOfInitializedData { get; set; }

        /// <summary>
        /// The size of the uninitialized data section (BSS), or the sum of all
        /// such sections if there are multiple BSS sections
        /// </summary>
        public uint SizeOfUninitializedData { get; set; }

        /// <summary>
        /// The address of the entry point relative to the image base when the 
        /// executable file is loaded into memory. For program images, this is 
        /// the starting address. For device drivers, this is the address of 
        /// the initialization function. An entry point is optional for DLLs. 
        /// When no entry point is present, this field must be zero
        /// </summary>
        public uint AddressOfEntryPoint { get; set; }

        /// <summary>
        /// The address that is relative to the image base of the beginning-of-code 
        /// section when it is loaded into memory
        /// </summary>
        public uint BaseOfCode { get; set; }
    }
}
