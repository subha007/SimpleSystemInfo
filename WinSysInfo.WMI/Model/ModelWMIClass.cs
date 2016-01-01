using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Model
{
    public class ModelWMIClass
    {
        public string ClassName;
        public List<ModelWMIProperty> Properties;

        public ModelWMIClass()
        {
            this.Properties = new List<ModelWMIProperty>();
        }
    }
}
