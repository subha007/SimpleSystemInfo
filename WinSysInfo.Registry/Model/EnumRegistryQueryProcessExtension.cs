namespace SysInfoInventryWinReg.Process
{
    /// <summary>
    /// The extension class for the enumeration class <see cref="EnumRegistryQueryProcess"/>
    /// </summary>
    public static class EnumRegistryQueryProcessExtension
    {
        /// <summary>
        /// The extension method checks if the subkeys are to be added too or not for first level.
        /// </summary>
        /// <param name="enumProc">The main object.</param>
        /// <returns>Returns true if the enum object contains the flag <see cref="ADD_ALL_SUBKEYS"/> or <see cref="ADD_ALL_SUBKEYS_RECURSIVE"/></returns>
        public static bool DoAddAllSubKeys(this EnumRegistryQueryProcess enumProc)
        {
            return (enumProc & EnumRegistryQueryProcess.ADD_ALL_SUBKEYS) == EnumRegistryQueryProcess.ADD_ALL_SUBKEYS;
        }

        /// <summary>
        /// The extension method checks if the flag <see cref="ADD_ALL_VALUES"/> is present or not.
        /// </summary>
        /// <param name="enumProc">The main object.</param>
        /// <returns>Returns true if the enum object contains the flag <see cref="ADD_ALL_VALUES"/>.</returns>
        public static bool DoAddAllValues(this EnumRegistryQueryProcess enumProc)
        {
            return ((enumProc & EnumRegistryQueryProcess.ADD_ALL_VALUES) == EnumRegistryQueryProcess.ADD_ALL_VALUES);
        }
    }
}
