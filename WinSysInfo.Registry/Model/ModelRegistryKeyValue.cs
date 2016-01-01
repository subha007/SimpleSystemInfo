using Microsoft.Win32;
using SysInfoException;
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
    /// The model class which represents a Value name and value list inside a key
    /// </summary>
    [DataContract]
    [XmlRoot("KeyValue")]
    public class ModelRegistryKeyValue : IExceptionMessageHandler
    {
        /// <summary>
        /// Get or set the value name
        /// </summary>
        [DataMember, XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Get or set the value type
        /// </summary>
        [DataMember, XmlAttribute("type")]
        public RegistryValueKind ValueType { get; set; }

        /// <summary>
        /// Get or set the list of values
        /// </summary>
        [DataMember, XmlElement("Data")]
        public List<string> Values { get; set; }

        /// <summary>
        /// Get or set the help topics
        /// </summary>
        [DataMember, XmlElement]
        public ModelHelpTopic Help { get; set; }

        /// <summary>
        /// Get or set the generic exception data handler for display purpose.
        /// </summary>
        [DataMember, XmlIgnore]
        public IExceptionMessage ExceptionData { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelRegistryKeyValue() { }

        /// <summary>
        /// Constructor to initialise the object with the unique name
        /// </summary>
        /// <param name="name">The name of the value key</param>
        public ModelRegistryKeyValue(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Parse the registry value
        /// </summary>
        /// <param name="value">Actual value</param>
        /// <param name="type">Type of data</param>
        public void ParseAndSetValue(object value, RegistryValueKind type)
        {
            this.Values = new List<string>();
            this.ValueType = type;
            switch (this.ValueType)
            {
                case RegistryValueKind.MultiString:
                    this.Values.AddRange((string[])value);
                    break;

                default:
                    this.Values.Add(value.ToString());
                    break;
            }
        }

        /// <summary>
        /// Add exception message to the object
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void AddLog(ExceptionLevel level, string message)
        {
            if (this.ExceptionData == null)
                this.ExceptionData = ExceptionFactory.GetNewInstance();

            this.ExceptionData.AddLog(level, message);
        }
    }
}
