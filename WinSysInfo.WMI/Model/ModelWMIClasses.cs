using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Model
{
    public class ModelWMIClasses
    {
        public ModelWMIPath Path { get; set; }
        public List<ModelWMIClass> Classes { get; set; }

        public ModelWMIClasses()
        {
            this.Path = new ModelWMIPath();
        }
    }
}
