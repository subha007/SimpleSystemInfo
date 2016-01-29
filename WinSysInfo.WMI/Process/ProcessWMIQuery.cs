using System;
using System.Management;
using System.Reflection;
using SysInfoWMI.Model;

namespace SysInfoWMI.Process
{
    /// <summary>
    /// This class is used to query root data
    /// </summary>
    public class ProcessWMIQuery
    {
        protected ManagementScope MgmntScope { get; set; }

        /// <summary>
        /// Initialize this object
        /// </summary>
        /// <param name="machineName">The full system name in the network</param>
        /// <param name="locations">The namespace path of Root</param>
        /// <param name="userName">The user Id used to connect</param>
        /// <param name="Password">The password</param>
        public ProcessWMIQuery(string computerName,
                               string[] locations,
                               string userName, string Password)
        {
            if(string.IsNullOrEmpty(computerName) == true)
                computerName = @"\\localhost";
            locations = locations ?? new string[] { "root", "CimV2" };

            ManagementPath mgmntPath = new ManagementPath(this.ConstructPath(computerName, locations));

            if(string.IsNullOrEmpty(userName) == true)
                this.MgmntScope = new ManagementScope(mgmntPath);
            else
            {
                ConnectionOptions connOptions = new ConnectionOptions
                {
                    Username = userName,
                    Password = Password
                };
                this.MgmntScope = new ManagementScope(mgmntPath, connOptions);
            }
        }

        /// <summary>
        /// Construct path
        /// </summary>
        /// <param name="computerName"></param>
        /// <param name="rootNamespace"></param>
        /// <returns></returns>
        protected string ConstructPath(string computerName, string [] locations)
        {
            string subPath = System.IO.Path.Combine(locations);

            if(string.IsNullOrEmpty(computerName) == true)
                computerName = @"\\localhost";
            else if(computerName.StartsWith(@"\\") == false)
                computerName = @"\\" + computerName;

            return System.IO.Path.Combine(computerName, subPath);
        }

        /// <summary>
        /// Get list of namespaces under a path
        /// </summary>
        public ModelWMINamespaces GetListofNamespaces()
        {
            ModelWMINamespaces objData = new ModelWMINamespaces();
            objData.Path = new ModelWMIPath(this.MgmntScope.Path.Server, 
                                            this.MgmntScope.Path.NamespacePath,
                                            this.MgmntScope.Path.ClassName);

            if(string.IsNullOrEmpty(this.MgmntScope.Path.ClassName) == false)
                return null;

            ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.MgmntScope,
                                                    new SelectQuery("__NAMESPACE"));

            ManagementObjectCollection objColl = searcher.Get();
            ManagementObjectCollection.ManagementObjectEnumerator objIndexer = objColl.GetEnumerator();

            while(objIndexer.MoveNext())
            {
                foreach(PropertyData prop in objIndexer.Current.Properties)
                {
                    objData.Lists.Add(prop.Value.ToString());
                }
            }

            return objData;
        }

        /// <summary>
        /// Get List of classes
        /// </summary>
        /// <returns></returns>
        public ModelWMIClasses GetListOfClasses()
        {
            ModelWMIClasses objData = new ModelWMIClasses();

            ManagementClass wmiclass = new ManagementClass(this.MgmntScope.Path);

            //Provides a base class for query and enumeration-related options objects
            EnumerationOptions options = new EnumerationOptions();

            //Return class members that are immediately related to the class
            options.EnumerateDeep = false;

            //Retrieve the  sbuclasses using the specified options
            foreach(ManagementObject mgmtObj in wmiclass.GetSubclasses(options))
            {
                ModelWMIClass objClass = new ModelWMIClass();
                objClass.ClassName = mgmtObj.ClassPath.ClassName;

                foreach(PropertyData prop in mgmtObj.Properties)
                {
                    ModelWMIProperty objProp = new ModelWMIProperty();
                    objProp.Name = prop.Name;
                    objProp.CimType = prop.Type;

                    objClass.Properties.Add(objProp);
                }
            }

            return objData;
        }

        public void QueryData(string className)
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(this.MgmntScope,
                new SelectQuery(className));

            ManagementObjectCollection objColl = searcher.Get();
            ManagementObjectCollection.ManagementObjectEnumerator objIndexer = objColl.GetEnumerator();

            while(objIndexer.MoveNext())
            {
                SetManagementBaseObject(objIndexer.Current);
            }
        }

        protected virtual void SetManagementBaseObject(ManagementBaseObject managementBaseObject)
        {
            throw new NotImplementedException();
        }

        protected void GetPropertyObject(PropertyDataCollection propColl, string keyName,
            object classObj)
        {
            try
            {
                object objSource = propColl[keyName].Value;
                Type type = classObj.GetType();
                PropertyInfo pi = type.GetProperty(keyName);
                if(pi != null && objSource != null)
                {
                    if(pi.PropertyType.IsGenericType == true)
                    {
                        Type mainType = Nullable.GetUnderlyingType(pi.PropertyType);
                        if(mainType != null)
                        {
                            if(mainType.IsEnum == true)
                            {
                                var enumType = mainType;
                                var enumValue = Enum.ToObject(enumType, objSource);
                                pi.SetValue(classObj, enumValue, null);
                            }
                            else
                                pi.SetValue(classObj, objSource);
                        }
                    }
                    else
                        pi.SetValue(classObj, objSource);
                }
            }
            catch(Exception) { }
        }

        /// <summary>
        /// Disconnect
        /// </summary>
        public void Disconnect()
        {
            if(this.MgmntScope != null || this.MgmntScope.IsConnected == true)
                this.MgmntScope = null;
        }
    }
}
