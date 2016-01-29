using System.Collections.Generic;

namespace SysInfoWMI.Model
{
    public class ModelWMINamespaces
    {
        public ModelWMIPath Path { get; set; }
        public List<string> Lists { get; set; }

        public ModelWMINamespaces()
        {
            this.Lists = new List<string>();
        }
    }
}
