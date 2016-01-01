using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Model
{
    public class ModelWMIWin32Product
    {
        /// <summary>
        /// Property returns the namespace of the WMI class.
        /// </summary>
        public string OriginatingNamespace
        {
            get
            {
                return "root\\CimV2";
            }
        }

        /// <summary>
        /// Private property to hold the name of WMI class which created this class.
        /// </summary>
        public static string CreatedClassName
        {
            get
            {
                return "Win32_Product";
            }
        }

        /// <summary>
        /// WMI system properties
        /// </summary>
        public ModelWMISystemProperties SystemProperties { get; set; }

        /// <summary>
        /// Assignment type of the product.
        /// </summary>
        public EnumAssignmentTypeValues? AssignmentType { get; set; }

        /// <summary>
        /// A short textual description (one-line string) for the Product.
        /// </summary>
        public string Caption { get; set; }

        /// <summary>
        /// A textual description of the product.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// The support link for the product.
        /// </summary>
        public string HelpLink { get; set; }

        /// <summary>
        /// The support telephone for the product.
        /// </summary>
        public string HelpTelephone { get; set; }

        /// <summary>
        /// Product identification such as a serial number on software, a die number on 
        /// a hardware chip, or (for non-commercial Products) a project number.
        /// </summary>
        public string IdentifyingNumber { get; set; }

        /// <summary>
        /// The installation date.  The InstallDate property has been deprecated in favor of 
        /// the InstallDate2 property which is of type DateTime rather than String. New impl 
        /// ementations should use the InstallDate2 property.
        /// </summary>
        public string InstallDate { get; set; }

        /// <summary>
        /// The InstallDate2 property represents the installation date of the product.
        /// </summary>
        public System.DateTime? InstallDate2 { get; set; }

        /// <summary>
        /// The location of the installed product.
        /// </summary>
        public string InstallLocation { get; set; }

        /// <summary>
        /// The installation source directory of the product.
        /// </summary>
        public string InstallSource { get; set; }

        /// <summary>
        /// The installed state of the product.
        /// </summary>
        public EnumInstallStateValues? InstallState { get; set; }

        /// <summary>
        /// The language of the product.
        /// </summary>
        public string Language { get; set; }

        /// <summary>
        /// The location of the locally cached package for this product.
        /// </summary>
        public string LocalPackage { get; set; }

        /// <summary>
        /// Commonly used product name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The location of the locally cached package for this product.
        /// </summary>
        public string PackageCache { get; set; }

        /// <summary>
        /// The identifier for the package from which this product was installed.
        /// </summary>
        public string PackageCode { get; set; }

        /// <summary>
        /// The original package name for the product.
        /// </summary>
        public string PackageName { get; set; }

        /// <summary>
        /// The product ID.
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// The company registered to use the product.
        /// </summary>
        public string RegCompany { get; set; }

        /// <summary>
        /// The owner registered to use the product.
        /// </summary>
        public string RegOwner { get; set; }

        /// <summary>
        /// Product SKU (stock keeping unit) information.
        /// </summary>
        public string SKUNumber { get; set; }

        /// <summary>
        /// The transforms of the product.
        /// </summary>
        public string Transforms { get; set; }

        /// <summary>
        /// The URL information for the product.
        /// </summary>
        public string URLInfoAbout { get; set; }

        /// <summary>
        /// The URL update information the product.
        /// </summary>
        public string URLUpdateInfo { get; set; }

        /// <summary>
        /// The name of the Product\'s supplier, or entity selling the Product (the manufactuer,
        /// reseller, OEM, etc.). Corresponds to the Vendor property in the Product object
        /// in the DMTF Solution Exchange Standard
        /// </summary>
        public string Vendor { get; set; }

        /// <summary>
        /// Product version information.  Corresponds to the Version property in the product
        /// object in the DMTF Solution Exchange Standard.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Summary information word count for the product.
        /// </summary>
        public uint? WordCount { get; set; }
    }
}
