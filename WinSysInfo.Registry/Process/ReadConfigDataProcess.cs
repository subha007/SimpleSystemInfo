using log4net;
using SysInfoInventry.Generic;
using SysInfoInventry.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SysInfoInventry.Process
{
    /// <summary>
    /// This class is used to read the xml file data from the App_Data folder.
    /// This is a generic class which uses the xml serializer and deserializer methods.
    /// </summary>
    public class ReadConfigDataProcess
    {
        /// <summary>
        /// Get the log instance for this class
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ReadConfigDataProcess));

        /// <summary>
        /// Get or set the configurator for the reading process
        /// </summary>
        protected RegistryConfigConfigurator Configurator { get; set; }

        /// <summary>
        /// Get the model of config file used to read registry settings
        /// </summary>
        public Dictionary<string, ModelRegistryKey> RegKeys
        {
            get;
            protected set;
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ReadConfigDataProcess()
        {
            this.Configurator = new RegistryConfigConfigurator();
        }

        /// <summary>
        /// Initialize the Reader process using the relative path of the Xml config file.
        /// </summary>
        /// <param name="relPath"></param>
        public ReadConfigDataProcess(string relPath)
        {
            this.Configurator = new RegistryConfigConfigurator(relPath);
        }


        /// <summary>
        /// Initialize the Reader process using the configurator object.
        /// </summary>
        /// <param name="configurator">the config filter object used for reading</param>
        public ReadConfigDataProcess(RegistryConfigConfigurator configurator)
        {
            this.Configurator = configurator;
        }

        /// <summary>
        /// The main method to read config file
        /// </summary>
        public void ReadConfig()
        {
            System.IO.FileInfo fiInfo = new System.IO.FileInfo(ConstantsXmlRegistryConfig.RelativePathRegsitryConfigFile);

            if (((fiInfo.Length / 1024.0) <= ConstantsConfig.LimitXmlFileReadKB)
                && (this.Configurator.RelativeXYPath.Count < 0))
            {
                ReadNormalFile();
            }
            else
            {
                if (this.Configurator.RelativeXYPath.Count < 0)
                {
                    ReadXPathFile();
                }
                else
                {
                    logger.Warn("Xml config file reading may take long time hence skipped.");
                }
            }
        }

        /// <summary>
        /// Read the xml config file full and desrialize
        /// </summary>
        private void ReadNormalFile()
        {
            System.Xml.Serialization.XmlSerializer reader =
                    new System.Xml.Serialization.XmlSerializer(typeof(ModelRegistryKey));

            // Create new FileStream with which to read the schema.
            XmlReader fsReadXml = XmlReader.Create(this.Configurator.RelativePath);
            try
            {
                RegKeys.Add(ConstantsXmlRegistryConfig.FullDeserializeKeyName, (ModelRegistryKey)reader.Deserialize(fsReadXml));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                fsReadXml.Close();
            }
        }

        /// <summary>
        /// Read the xml config file partially
        /// </summary>
        private void ReadXPathFile()
        {
            using (XmlReader reader = XmlReader.Create(this.Configurator.RelativePath))
            {
                Read(reader);
            }
        }

        /// <summary>
        /// Read the xml config file using Xml Reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private void Read(XmlReader reader)
        {
            RegKeys = new Dictionary<string, ModelRegistryKey>();

            while (reader.ReadToFollowing(ConstantsXmlRegistryConfig.PathXmlTag) == true)
            {
                if (reader.HasAttributes == false)
                    throw new XmlException("Invalid xml config data");

                string pathNameID = reader.GetAttribute(ConstantsXmlRegistryConfig.NameAttributeXmlTag);

                if (this.Configurator.RelativeXYPath.ContainsKey(pathNameID) == false)
                    throw new XmlException("Invalid xml config data");

                using (StringReader elementReader = new StringReader(reader.ReadOuterXml()))
                {
                    XmlSerializer deserializer = new XmlSerializer(typeof(ModelRegistryKey));
                    RegKeys.Add(pathNameID, (ModelRegistryKey)deserializer.Deserialize(elementReader));
                }
            }
        }
    }
}
