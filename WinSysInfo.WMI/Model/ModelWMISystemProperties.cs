namespace SysInfoWMI.Model
{
    /// <summary>
    /// Embedded class to represent WMI system Properties.
    /// </summary>
    public class ModelWMISystemProperties
    {
        /// <summary>
        /// Property value __GENUS
        /// </summary>
        public int GENUS { get; set; }

        /// <summary>
        /// Property value __CLASS
        /// </summary>
        public string CLASS { get; set; }

        /// <summary>
        /// Property value __SUPERCLASS
        /// </summary>
        public string SUPERCLASS { get; set; }

        /// <summary>
        /// Property value __DYNASTY
        /// </summary>
        public string DYNASTY { get; set; }

        /// <summary>
        /// Property value __RELPATH
        /// </summary>
        public string RELPATH { get; set; }

        /// <summary>
        /// Property value __PROPERTY_COUNT
        /// </summary>
        public string PROPERTY_COUNT { get; set; }

        /// <summary>
        /// Property value __DERIVATION
        /// </summary>
        public string[] DERIVATION { get; set; }

        /// <summary>
        /// Property value __SERVER
        /// </summary>
        public string SERVER { get; set; }

        /// <summary>
        /// Property value __NAMESPACE
        /// </summary>
        public string NAMESPACE { get; set; }

        /// <summary>
        /// Property value __PATH
        /// </summary>
        public string PATH { get; set; }
    }
}
