using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
