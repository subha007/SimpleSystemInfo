﻿namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The following relocation type indicators are defined for x64 
    /// and compatible processors
    /// </summary>
    public enum EnumRelocationTypeAMD64
    {
        IMAGE_REL_AMD64_ABSOLUTE = 0x0000,
        IMAGE_REL_AMD64_ADDR64 = 0x0001,
        IMAGE_REL_AMD64_ADDR32 = 0x0002,
        IMAGE_REL_AMD64_ADDR32NB = 0x0003,
        IMAGE_REL_AMD64_REL32 = 0x0004,
        IMAGE_REL_AMD64_REL32_1 = 0x0005,
        IMAGE_REL_AMD64_REL32_2 = 0x0006,
        IMAGE_REL_AMD64_REL32_3 = 0x0007,
        IMAGE_REL_AMD64_REL32_4 = 0x0008,
        IMAGE_REL_AMD64_REL32_5 = 0x0009,
        IMAGE_REL_AMD64_SECTION = 0x000A,
        IMAGE_REL_AMD64_SECREL = 0x000B,
        IMAGE_REL_AMD64_SECREL7 = 0x000C,
        IMAGE_REL_AMD64_TOKEN = 0x000D,
        IMAGE_REL_AMD64_SREL32 = 0x000E,
        IMAGE_REL_AMD64_PAIR = 0x000F,
        IMAGE_REL_AMD64_SSPAN32 = 0x0010
    }
}
