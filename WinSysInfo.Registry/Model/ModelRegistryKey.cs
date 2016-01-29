using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using WinSysInfo.WSIException;

namespace SysInfoInventryWinReg.Model
{
    /// <summary>
    /// The class which replicates the data from Windows Registry
    /// </summary>
    [DataContract]
    [XmlRoot("ModelRegistry")]
    public class ModelRegistryKey : IExceptionMessageHandler
    {
        /// <summary>
        /// Get or set the main root path of the registry to query
        /// </summary>
        public ModelRegistryPath RegsitryPath { get; set; }

        /// <summary>
        /// Get or set the Tree level of this Registry Key. The root Node starts from 0.
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public uint TreeLevel { get; set; }

        /// <summary>
        /// Get or set the help topics
        /// </summary>
        [DataMember, XmlElement]
        public ModelHelpTopic Help { get; set; }

        /// <summary>
        /// Get or set the list of values under the Key
        /// </summary>
        [DataMember, XmlArray("KeyValues"), XmlArrayItem("KeyValue")]
        public List<ModelRegistryKeyValue> KeyValuePairs { get; set; }

        /// <summary>
        /// Get or set the list of subkeys to query
        /// </summary>
        [DataMember, XmlArray("SubKeys"), XmlArrayItem("SubKey")]
        public List<ModelRegistryKey> SubKeys { get; set; }

        /// <summary>
        /// Get or set the generic exception data handler for display purpose.
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public IExceptionMessage ExceptionData { get; set; }

        /// <summary>
        /// Default parameterless constructor
        /// </summary>
        public ModelRegistryKey() { }

        /// <summary>
        /// Default constructor to initialize the model from a parent model and subkey name and tree level
        /// </summary>
        /// <param name="parentKey">The parent subkey object</param>
        /// <param name="uniqueName">The unique name of the subkey</param>
        /// <param name="level">The level relative to the top root node.</param>
        public ModelRegistryKey(ModelRegistryKey parentKey, string uniqueName, uint level)
        {
            this.RegsitryPath = new ModelRegistryPath(parentKey.RegsitryPath, uniqueName);
            this.TreeLevel = level;
        }

        /// <summary>
        /// Constructor to initialize the model from the registry path
        /// </summary>
        /// <param name="regPath"></param>
        public ModelRegistryKey(ModelRegistryPath regPath)
        {
            this.RegsitryPath = regPath;
            this.TreeLevel = 0;
        }

        /// <summary>
        /// Find and get the value object if not found create a new instance.
        /// </summary>
        /// <param name="uniqueName">The unique name of the value</param>
        public ModelRegistryKeyValue GetValueObject(string uniqueName)
        {
            ModelRegistryKeyValue regValue = null;
            if(this.KeyValuePairs != null)
                regValue = this.KeyValuePairs
                                .FirstOrDefault(x => x != null && x.Name == uniqueName);
            else
                this.KeyValuePairs = new List<ModelRegistryKeyValue>();

            if(regValue == null)
            {
                regValue = new ModelRegistryKeyValue(uniqueName);
                this.KeyValuePairs.Add(regValue);
            }

            return regValue;
        }

        /// <summary>
        /// Find and get the Subkey object if not found create a new instance
        /// </summary>
        /// <param name="uniqueName">The unique name of the value</param>
        public ModelRegistryKey GetSubKeyObject(string uniqueName)
        {
            ModelRegistryKey subkey = null;

            if(this.SubKeys != null)
                subkey = this.SubKeys
                             .FirstOrDefault(x => x != null && x.RegsitryPath != null &&
                                (x.RegsitryPath.IsSubKeyFound(uniqueName) == true));
            else
                this.SubKeys = new List<ModelRegistryKey>();

            if(subkey == null)
            {
                subkey = new ModelRegistryKey(this, uniqueName, this.TreeLevel + 1);
                this.SubKeys.Add(subkey);
            }

            return subkey;
        }

        /// <summary>
        /// Add exception message to the object
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void AddLog(ExceptionLevel level, string message)
        {
            //if(this.ExceptionData == null)
            //    this.ExceptionData = ExFactoryGeneric.GetNewInstance();

            //this.ExceptionData.AddLog(level, message);
        }

        /// <summary>
        /// Validate the object on the basis of data
        /// </summary>
        /// <returns></returns>
        public bool Validate(bool throwExcp)
        {
            if(this.RegsitryPath == null)
                if(throwExcp) throw new Exception("Registry Path null or empty is not allowed.");
                else return false;

            if(this.TreeLevel < 0)
                if(throwExcp) throw new Exception("Tree Level value should be whole numbers.");
                else return false;

            return true;
        }
    }
}
