using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoInventryWinReg.Attribute
{
    /// <summary>
    /// The class used for Xml Serialization of Dictionary object
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field |
                       System.AttributeTargets.Property)]
    public class XmlDictionaryKeyValueAttribute : System.Attribute
    {
        /// <summary>
        /// Get or set if Key is also serializable
        /// </summary>
        public bool IsKeySerializable { get; set; }

        /// <summary>
        /// Get or set the Xml tag name for Key. Only applicable if <see cref="IsKeySerializable"/>
        /// is true.
        /// </summary>
        public string KeyTagName { get; set; }

        /// <summary>
        /// Get or set if Value is also serializable
        /// </summary>
        public bool IsValueSerializable { get; set; }

        /// <summary>
        /// Get or set the Xml tag name for Value. Only applicable if <see cref="IsValueSerializable"/>
        /// is true.
        /// </summary>
        public string ValueTagName { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public XmlDictionaryKeyValueAttribute() 
        {
            this.IsKeySerializable = true;
            this.KeyTagName = "Key";
            this.IsValueSerializable = true;
            this.ValueTagName = "Value";
        }
    }
}
