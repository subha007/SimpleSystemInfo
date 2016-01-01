using SysInfoInventryWinReg.Generic;
using SysInfoInventryWinReg.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SysInfoInventryWinReg.Process
{
    /// <summary>
    /// The class which processes the Registry config file.
    /// It reads the config data into the model class depending upon the filter criteria class
    /// <see cref="ConfiguratorRegistryConfig"/>
    /// </summary>
    public class ProcessRegistryConfigData : ProcessConfigData<ModelRegistryKey>
    {
        /// <summary>
        /// Default constructor to initialize the process to read registry config data
        /// </summary>
        /// <param name="relPath"></param>
        public ProcessRegistryConfigData(string relPath)
        {
            this.Configurator = new ConfiguratorRegistryConfig(relPath);
        }

        /// <summary>
        /// Default constructor to initialize the process to read registry config data
        /// </summary>
        /// <param name="relPath"></param>
        public ProcessRegistryConfigData(ConfiguratorRegistryConfig configurator)
        {
            this.Configurator = configurator;
        }

        /// <summary>
        /// Read the xml config file using Xml Reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected override void Read(XmlReader reader)
        {
            while (reader.ReadToFollowing(this.Configurator.ConfigPath) == true)
            {
                if (reader.HasAttributes == false)
                    throw new XmlException("Invalid xml config data");

                string pathNameID = reader.GetAttribute(ConstantsXmlRegistryConfig.NameAttributeXmlTag);

                if (((ConfiguratorRegistryConfig)this.Configurator).RelativeXYPath.ContainsKey(pathNameID) == false)
                    throw new XmlException("Invalid xml config data");

                using (MemoryStream elementReader = new MemoryStream(Encoding.Unicode.GetBytes(reader.ReadOuterXml())))
                {
                    XmlSerializer serialize = new XmlSerializer(typeof(ModelRegistryKey));
                    this.ModelDataDictionary.Add(pathNameID, (ModelRegistryKey)serialize.Deserialize(elementReader));
                }
            }
        }

        /// <summary>
        /// Write the xml object to file
        /// </summary>
        public override void Write()
        {
            base.Write();

            if (this.ModelDataDictionary.Count == 1 &&
                this.ModelDataDictionary.ContainsKey(ConstantsXmlRegistryConfig.FullDeserializeKeyName) == true)
            {
                WriteNormalFile();
            }
            else if (this.ModelDataDictionary.Count > 0 &&
                this.ModelDataDictionary.ContainsKey(ConstantsXmlRegistryConfig.FullDeserializeKeyName) == false)
            {
                WriteFragmentedFile();
            }
            else
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Write fragmented file
        /// </summary>
        private void WriteFragmentedFile()
        {
            using (XmlWriter xmlWriter = this.GetXmlWriterInstance())
            {
                xmlWriter.WriteStartDocument();
                xmlWriter.WriteStartElement(ConstantsXmlRegistryConfig.PathXmlTag);

                foreach (KeyValuePair<string, ModelRegistryKey> keyVal in this.ModelDataDictionary)
                {
                    XmlSerializer serialize = new XmlSerializer(typeof(ModelRegistryKey));
                    using(MemoryStream memory = new MemoryStream())
                    {
                        serialize.Serialize(memory, keyVal.Value, this.GetXmlNamespaces());
                        xmlWriter.WriteRaw(memory.ToString());
                    }
                }

                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
            }
        }

        /// <summary>
        /// Write the xml to normal
        /// </summary>
        private void WriteNormalFile()
        {
            using (XmlWriter xmlWriter = this.GetXmlWriterInstance())
            {
                XmlSerializer serialize = new XmlSerializer(typeof(ModelRegistryKey));
                serialize.Serialize(xmlWriter, this.ModelDataDictionary[ConstantsXmlRegistryConfig.FullDeserializeKeyName], this.GetXmlNamespaces());
            }
        }

        /// <summary>
        /// An internal method to get an Xml Writer instance
        /// </summary>
        /// <returns></returns>
        private XmlWriter GetXmlWriterInstance()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = false;
            settings.Indent = true;
            settings.IndentChars = " ";

            return XmlWriter.Create(this.Configurator.ConfigPath, settings);
        }

        private XmlSerializerNamespaces GetXmlNamespaces()
        {
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");

            return ns;
        }
    }
}
