using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Model
{
    public class ModelWMIProperty
    {
        public string Name { get; set; }
        public System.Management.CimType CimType { get; set; }
        public Type DotNetType { get; set; }
    }
}
