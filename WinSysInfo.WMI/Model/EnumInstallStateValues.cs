using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Model
{
    public enum EnumInstallStateValues
    {
        Bad_Configuration = -6,
        Invalid_Argument = -2,
        Unknown_Package = -1,
        Advertised = 1,
        Absent = 2,
        Installed = 5,
        NULL_ENUM_VALUE = 0,
    }
}
