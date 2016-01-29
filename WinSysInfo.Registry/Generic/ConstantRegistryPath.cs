using SysInfoInventryWinReg.Model;

namespace SysInfoInventryWinReg.Generic
{
    public static class ConstantRegistryPath
    {
        /// <summary>
        /// Get the fixed registry path for registry query in case it is not defined
        /// </summary>
        public static readonly ModelRegistryPath UninstalledRegistryPath
            = new ModelRegistryPath(Microsoft.Win32.RegistryHive.LocalMachine, @"Software\Microsoft\Windows\CurrentVersion\Uninstall");
    }
}
