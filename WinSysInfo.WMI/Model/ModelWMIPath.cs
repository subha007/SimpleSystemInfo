using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Model
{
    public class ModelWMIPath
    {
        public string Server { get; set; }
        public string Namespace { get; set; }
        public string ClassName { get; set; }

        public ModelWMIPath() { }

        public ModelWMIPath(string server, string namespacePath, string className)
        {
            this.Server = server;
            this.Namespace = namespacePath;
            this.ClassName = className;
        }
    }
}
