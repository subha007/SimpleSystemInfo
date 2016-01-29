﻿namespace WinSysInfo.PEView.Model
{
    /// <summary>
    /// The following values defined for the Subsystem field of the optional
    /// header determine which Windows subsystem (if any) is required to run the image.
    /// </summary>
    public enum EnumWindowsSubsytem
    {
        /// <summary>
        /// An unknown subsystem
        /// </summary>
        IMAGE_SUBSYSTEM_UNKNOWN = 0,

        /// <summary>
        /// Device drivers and native Windows processes
        /// </summary>
        IMAGE_SUBSYSTEM_NATIVE = 1,

        /// <summary>
        /// The Windows graphical user interface (GUI) subsystem
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_GUI = 2,

        /// <summary>
        /// The Windows character subsystem
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_CUI = 3,

        /// <summary>
        /// The OS/2 character subsytem
        /// </summary>
        IMAGE_SUBSYSTEM_OS2_CUI = 5,

        /// <summary>
        /// The Posix character subsystem
        /// </summary>
        IMAGE_SUBSYSTEM_POSIX_CUI = 7,

        /// <summary>
        /// Native Windows 9x driver.
        /// </summary>
        IMAGE_SUBSYSTEM_NATIVE_WINDOWS = 8,

        /// <summary>
        /// Windows CE
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_CE_GUI = 9,

        /// <summary>
        /// An Extensible Firmware Interface (EFI) application
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_APPLICATION = 10,

        /// <summary>
        /// An EFI driver with boot services
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_BOOT_SERVICE_DRIVER = 11,

        /// <summary>
        /// An EFI driver with run-time services
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_RUNTIME_DRIVER = 12,

        /// <summary>
        /// An EFI ROM image
        /// </summary>
        IMAGE_SUBSYSTEM_EFI_ROM = 13,

        /// <summary>
        /// XBOX
        /// </summary>
        IMAGE_SUBSYSTEM_XBOX = 14,

        /// <summary>
        /// Windows Boot application. A BCD application.
        /// </summary>
        IMAGE_SUBSYSTEM_WINDOWS_BOOT_APPLICATION = 16
    }
}
