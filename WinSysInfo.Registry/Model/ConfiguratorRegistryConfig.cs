using SysInfoInventryWinReg.Generic;
using SysInfoInventryWinReg.Process;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SysInfoInventryWinReg.Model
{
    /// <summary>
    /// The class to be used in filter of reading of registry xml config
    /// <see cref="ProcessRegistryConfigData"/>
    /// Based on the forum
    /// <see cref="http://stackoverflow.com/questions/676274/what-is-the-best-way-to-parse-big-xml-in-c-sharp-code"/>
    /// And MSDN <see cref="https://msdn.microsoft.com/en-US/library/ms182372.aspx"/>
    /// </summary>
    public class ConfiguratorRegistryConfig : ConfiguratorBaseConfig
    {
        /// <summary>
        /// Get or set the list of identifiers mentioned in Xml config
        /// Format is e.g., Path[Pathname11][Valuename11, ...];Path[Pathname21, ...][Valuename21, ...]
        /// Where 
        /// 1. Path = Xml element tag name which depicts a Reqgitry path identifier
        /// 2. Pathname{i} = The path name value in the 'name' attribute
        /// 3. Valuename{j} = The list of key value names needed
        /// </summary>
        public Dictionary<string, List<string>> RelativeXYPath { get; set; }

        /// <summary>
        /// Default constructor to initialize the registry config filter process data
        /// </summary>
        /// <param name="useConfig">If to use the filter config or skip.</param>
        /// <param name="relPath">The relative path of config file. Relative to the current environment path or the application path.</param>
        /// <param name="xypath">The xy path configurator</param>
        public ConfiguratorRegistryConfig(bool useConfig, string relPath, string xypath)
            : base(useConfig, relPath)
        {
            // Initialize the object
            this.RelativeXYPath = new Dictionary<string, List<string>>();
            TryParseXYPath(xypath);
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfiguratorRegistryConfig() : this(true, string.Empty, string.Empty) { }

        /// <summary>
        /// Default constructor with the relative xml file Path
        /// </summary>
        /// <param name="relPath">The relative path of config file. Relative to the current environment path or the application path.</param>
        /// <param name="xypath">The xy path configurator</param>
        public ConfiguratorRegistryConfig(string relPath, string xypath) : this(true, relPath, xypath) { }

        /// <summary>
        /// Default constructor with relative path of the config file
        /// </summary>
        /// <param name="relPath"></param>
        public ConfiguratorRegistryConfig(string relPath) : this(true, relPath, string.Empty) { }

        /// <summary>
        /// Manipulated property to check if the config file is to be read as a normal file or as partial
        /// </summary>
        public override bool IsConfigReadAsNormalFile
        {
            get
            {
                System.IO.FileInfo fiInfo = new System.IO.FileInfo(ConstantsXmlRegistryConfig.RelativePathRegsitryConfigFile);
                return (((fiInfo.Length / 1024.0) <= ConstantsConfig.LimitXmlFileReadKB)
                            && (this.IsConfigReadAsPartialFile == false));
            }
        }

        /// <summary>
        /// Manipulated property to check if the config file is to be read as a partial
        /// </summary>
        public override bool IsConfigReadAsPartialFile
        {
            get
            {
                return this.RelativeXYPath.Count > 0;
            }
        }

        /// <summary>
        /// Get or set the list of identifiers mentioned in Xml config
        /// Format is e.g., Path[Pathname11][Valuename11, ...];Path[Pathname21, ...][Valuename21, ...]
        /// Where 
        /// 1. Path = Xml element tag name which depicts a Reqgitry path identifier
        /// 2. Pathname{i} = The path name value in the 'name' attribute
        /// 3. Valuename{j} = The list of key value names needed
        /// Regex is : (Path)\[(?<Pathname>\w+)\]\[(?<Valuename>\w+\,*\w+)\]
        /// </summary>
        private void TryParseXYPath(string xpath)
        {
            // If no xypath defined then full config file to be read
            if (string.IsNullOrEmpty(xpath) == true)
                return;

            Regex regexXYPath = new Regex(ConstantsXmlRegistryConfig.RegexForConfiguratorXYPath, RegexOptions.Singleline);
            Match matchXYPath = regexXYPath.Match(xpath);
            if(matchXYPath.Success == true)
            {
                if (matchXYPath.Groups.Count > 0)
                    throw new KeyNotFoundException("No data found to parse means data is not correct or regex is wrong.");

                if (string.Compare(matchXYPath.Groups[ConstantsXmlRegistryConfig.RegexXmlTagGroup].Value, 
                    ConstantsXmlRegistryConfig.PathXmlTag, StringComparison.OrdinalIgnoreCase) != 0)
                    throw new KeyNotFoundException(string.Format("{0} not found", ConstantsXmlRegistryConfig.PathXmlTag));

                for(int indxData = 0; indxData < matchXYPath.Groups.Count; ++indxData)
                {
                    Group regexGroup = matchXYPath.Groups[indxData];
                    for(int indxPaths = 0; indxPaths < matchXYPath.Groups.Count; ++indxPaths)
                    {
                        Capture regexPath = regexGroup.Captures[indxPaths];
                        //TODO
                    }
                }
            }
        }
    }
}
