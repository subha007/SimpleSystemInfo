using Microsoft.Win32;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace SysInfoInventryWinReg.Model
{
    /// <summary>
    /// This class represents a Regsitry Key path
    /// </summary>
    [DataContract]
    [XmlRoot("ModelRegistry")]
    public class ModelRegistryPath
    {
        /// <summary>
        /// Get or set the main root path of the registry to query
        /// </summary>
        [DataMember, XmlAttribute("root")]
        public RegistryHive RootPath { get; set; }

        /// <summary>
        /// Get or set the subkey path of the registry to query
        /// </summary>
        [DataMember, XmlAttribute("path")]
        public string SubKeyPath { get; set; }

        /// <summary>
        /// Get the Sub key name
        /// </summary>
        [IgnoreDataMember, XmlIgnore]
        public string SubKeyName
        {
            get
            {
                if (string.IsNullOrEmpty(this.SubKeyPath) == true)
                    return string.Empty;
                int index = this.SubKeyPath.TrimEnd(new char[] { '\\' }).LastIndexOf('\\');
                if (index <= 0 || index >= this.SubKeyPath.Length)
                    return string.Empty;
                else
                    return this.SubKeyPath.Substring(index + 1, this.SubKeyPath.Length - (index + 1));
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ModelRegistryPath() { }

        /// <summary>
        /// Constructor to create object using  all information
        /// </summary>
        /// <param name="rootPath"></param>
        /// <param name="path"></param>
        public ModelRegistryPath(RegistryHive rootPath, string path)
        {
            this.RootPath = rootPath;
            this.SubKeyPath = path;
        }

        /// <summary>
        /// Constructor to construct using parent Registry path
        /// </summary>
        /// <param name="parent">The parent Key</param>
        /// <param name="uniqueName">The name of subkey</param>
        public ModelRegistryPath(ModelRegistryPath parent, string uniqueName)
        {
            this.RootPath = parent.RootPath;
            this.SubKeyPath = Path.Combine(parent.SubKeyPath, uniqueName);
        }

        /// <summary>
        /// Check if the subkey is the same
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsSubKeyFound(string name)
        {
            return (this.SubKeyName == name);
        }

        /// <summary>
        /// Validate the object on the basis of data
        /// </summary>
        /// <returns></returns>
        public bool Validate(bool throwExcp)
        {
            if (string.IsNullOrEmpty(this.SubKeyPath) == true)
                if (throwExcp) throw new Exception("Sub Key null or empty is not allowed");
                else return false;

            return true;
        }
    }
}
