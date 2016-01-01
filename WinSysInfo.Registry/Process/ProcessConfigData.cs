using log4net;
using SysInfoInventryWinReg.Generic;
using SysInfoInventryWinReg.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SysInfoInventryWinReg.Process
{
    /// <summary>
    /// This class is used to read the xml file data from the App_Data folder.
    /// This is a generic class which uses the xml serializer and deserializer methods depending upon the size of the config
    /// file. The limit of the file size is specified in the application config.
    /// </summary>
    public class ProcessConfigData<T> where T : new()
    {
        /// <summary>
        /// Get the log instance for this class to create log.
        /// </summary>
        private static ILog logger = LogManager.GetLogger(typeof(ProcessConfigData<T>));

        /// <summary>
        /// Get the model of config file used to store the data
        /// </summary>
        public Dictionary<string, T> ModelDataDictionary
        {
            get;
            protected set;
        }

        /// <summary>
        /// Get or set the configurator for the config data process
        /// </summary>
        protected virtual ConfiguratorBaseConfig Configurator { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProcessConfigData() : this(string.Empty) { }

        /// <summary>
        /// Constructor using the relative path of the Xml config file.
        /// </summary>
        /// <param name="relPath"></param>
        public ProcessConfigData(string relPath)
        {
            this.Configurator = new ConfiguratorBaseConfig(relPath);
        }

        /// <summary>
        /// Constructor using the configurator object.
        /// </summary>
        /// <param name="configurator">the config filter object used for reading</param>
        public ProcessConfigData(ConfiguratorBaseConfig configurator)
        {
            this.Configurator = configurator;
        }

        /// <summary>
        /// Add model data object
        /// </summary>
        /// <param name="objData"></param>
        public void AddModelObject(string key, T objData)
        {
            if (this.ModelDataDictionary == null)
                this.ModelDataDictionary = new Dictionary<string, T>();

            if (this.ModelDataDictionary.ContainsKey(key) == true)
                this.ModelDataDictionary[key] = objData;
            else
                this.ModelDataDictionary.Add(key, objData);
        }

        /// <summary>
        /// The main method to read config file
        /// </summary>
        public void ReadConfig()
        {
            this.ModelDataDictionary = new Dictionary<string, T>();

            if (this.Configurator.IsConfigReadAsNormalFile)
            {
                // The size of the file is less than the limit specified in application config file
                // And no XY Path specified to specific read
                // Then parse the whole file
                ReadNormalFile();
            }
            else
            {
                if (this.Configurator.IsConfigReadAsPartialFile == true)
                {
                    ReadXPathFile();
                }
                else
                {
                    logger.Warn("Xml config file reading may take long time hence skipped.");

                    // override any settings and read nortmally
                    ReadNormalFile();
                }
            }
        }

        /// <summary>
        /// Read the xml config file full and desrialize
        /// </summary>
        protected virtual void ReadNormalFile()
        {
            XmlSerializer serialize = new XmlSerializer(typeof(T));

            // Create new FileStream with which to read the schema.
            using (FileStream fsReadXml = new FileStream(this.Configurator.ConfigPath, FileMode.Open))
            {
                try
                {
                    this.ModelDataDictionary.Add(ConstantsXmlRegistryConfig.FullDeserializeKeyName,
                        (T)serialize.Deserialize(fsReadXml));
                }
                catch (Exception ex)
                {
                    logger.Error("Error while reading the config file in normal mode. " + ex.ToString());
                }
            }
        }

        /// <summary>
        /// Read the xml config file partially
        /// </summary>
        private void ReadXPathFile()
        {
            using (XmlReader reader = XmlReader.Create(this.Configurator.ConfigPath))
            {
                Read(reader);
            }
        }

        /// <summary>
        /// Read the xml config file using Xml Reader
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        protected virtual void Read(XmlReader reader)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Write xml from model object
        /// </summary>
        public virtual void Write()
        {
            if (this.ModelDataDictionary == null || this.ModelDataDictionary.Count <= 0)
            {
                logger.Error("No data model to write");
                return;
            }
        }
    }
}
