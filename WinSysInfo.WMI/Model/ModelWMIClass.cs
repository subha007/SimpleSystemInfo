using System.Collections.Generic;

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
