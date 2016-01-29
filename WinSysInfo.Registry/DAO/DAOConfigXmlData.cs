using SysInfoInventryWinReg.Model;

namespace SysInfoInventryWinReg.DAO
{
    /// <summary>
    /// This static class contains the Xml config files and held in memory
    /// </summary>
    public class DAOConfigXmlData
    {
        /// <summary>
        /// Get or set the config xml data in memory
        /// </summary>
        public ModelRegistryKey ConfigXmlRegistry { get; set; }
    }
}
