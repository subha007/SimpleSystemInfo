using System.Collections.Generic;

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
