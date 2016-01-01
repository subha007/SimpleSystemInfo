using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SysInfoInventryWinReg.Model
{
    /// <summary>
    /// The generic help topics used
    /// </summary>
    [DataContract]
    [XmlRoot("HelpTopic")]
    public class ModelHelpTopic
    {
        /// <summary>
        /// Get or set the Long description
        /// </summary>
        [DataMember, XmlElement]
        public string Description { get; set; }

        /// <summary>
        /// Get or set the short description
        /// </summary>
        [DataMember, XmlElement]
        public string ShortDescription { get; set; }

        /// <summary>
        /// Get or set the long name
        /// </summary>
        [DataMember, XmlElement]
        public string LongName { get; set; }
    }
}
