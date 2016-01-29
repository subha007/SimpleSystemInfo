using Microsoft.Win32;

namespace SysInfoInventryWinReg.Generic
{
    /// <summary>
    /// This class is to be used to access the constants in Registry Xml Config file
    /// </summary>
    public static class ConstantsXmlRegistryConfig
    {
        /// <summary>
        /// The IISVersion key name
        /// </summary>
        public const string RelativePathRegsitryConfigFile = @"App_Data\Config\RegsitryKeyConfig.xml";

        /// <summary>
        /// The XPath generic value with format specifier for the Path name attribute
        /// </summary>
        public const string XPathToPathName = @"//ModelRegistry/Path/[@name='{0}'";

        /// <summary>
        /// The Xml tag for Path in config
        /// </summary>
        public const string PathXmlTag = "Path";

        /// <summary>
        /// The Xml attribute tag for name in config
        /// </summary>
        public const string NameAttributeXmlTag = "name";

        /// <summary>
        /// Just a internal constant to store full config data
        /// </summary>
        public const string FullDeserializeKeyName = "FULL_DESERIALIZE";

        /// <summary>
        /// The path name for IISVersion
        /// </summary>
        public const string IISVersionPathName = "IISVersion";

        /// <summary>
        /// The path name for ODBCDrivers
        /// </summary>
        public const string ODBCDriversPathName = "ODBCDrivers";

        /// <summary>
        /// The path name for ODBCDrivers
        /// </summary>
        public const string ODBCDriversINIPathName = "ODBCDriversINI";

        /// <summary>
        /// The regex to parse the data passed to
        /// </summary>
        public const string RegexForConfiguratorXYPath = @"(?<xmltag>Path)\[(?<Pathname>\w+)\](\[(?<Valuename>\w+\,*\w+)\])?";

        /// <summary>
        /// List of group name used with the regex in constant variable <see cref="RegexForConfiguratorXYPath"/>
        /// </summary>
        public const string RegexXmlTagGroup = "xmltag";
        public const string RegexPathnameGroup = "Pathname";
        public const string RegexValuenameGroup = "Valuename";

        /// <summary>
        /// Convert from string to RegistryHive
        /// </summary>
        /// <param name="regHiveName"></param>
        /// <returns></returns>
        public static RegistryHive ConvertTo(string regHiveName)
        {
            switch(regHiveName)
            {
                case "HKEY_CLASSES_ROOT":
                    return RegistryHive.ClassesRoot;

                case "HKEY_CURRENT_USER":
                    return RegistryHive.CurrentUser;

                case "HKEY_LOCAL_MACHINE":
                    return RegistryHive.LocalMachine;

                case "HKEY_USERS":
                    return RegistryHive.Users;

                case "HKEY_PERFORMANCE_DATA":
                    return RegistryHive.PerformanceData;

                case "HKEY_CURRENT_CONFIG":
                    return RegistryHive.CurrentConfig;

                default:
                    return RegistryHive.LocalMachine;
            }
        }

        /// <summary>
        /// Convert from RegistryHive to string
        /// </summary>
        /// <param name="regHive"></param>
        /// <returns></returns>
        public static string ConvertTo(RegistryHive regHive)
        {
            switch (regHive)
            {
                case RegistryHive.ClassesRoot:
                    return "HKEY_CLASSES_ROOT";

                case RegistryHive.CurrentUser:
                    return "HKEY_CURRENT_USER";

                case RegistryHive.LocalMachine:
                    return "HKEY_LOCAL_MACHINE";

                case RegistryHive.Users:
                    return "HKEY_USERS";

                case RegistryHive.PerformanceData:
                    return "HKEY_PERFORMANCE_DATA";

                case RegistryHive.CurrentConfig:
                    return "HKEY_CURRENT_CONFIG";

                default:
                    return "HKEY_LOCAL_MACHINE";
            }
        }
    }
}
