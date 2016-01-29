using System.Collections.Generic;
using System.Management;
using SysInfoWMI.Model;

namespace SysInfoWMI.Process
{
    public class ProcessWMIProductQuery : ProcessWMIQuery
    {
        public List<ModelWMIWin32Product> Products { get; set; }

        public ProcessWMIProductQuery()
            : base(null, null, null, null)
        {
            this.Products = new List<ModelWMIWin32Product>();
        }

        public void QueryData()
        {
            base.QueryData("Win32_Product");
        }

        protected override void SetManagementBaseObject(ManagementBaseObject mgmntObj)
        {
            ModelWMIWin32Product product = new ModelWMIWin32Product();

            foreach(PropertyData prop in mgmntObj.Properties)
                this.GetPropertyObject(mgmntObj.Properties, prop.Name, product);

            this.Products.Add(product);
        }
    }
}
