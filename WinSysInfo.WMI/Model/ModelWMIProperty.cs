using System;

namespace SysInfoWMI.Model
{
    public class ModelWMIProperty
    {
        public string Name { get; set; }
        public System.Management.CimType CimType { get; set; }
        public Type DotNetType { get; set; }
    }
}
