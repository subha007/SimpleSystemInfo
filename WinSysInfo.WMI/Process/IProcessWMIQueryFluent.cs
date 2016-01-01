using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace SysInfoWMI.Process
{
    /// <summary>
    /// Used in fluent interface
    /// </summary>
    public interface IProcessWMIQueryFluent
    {
        /// <summary>
        /// Connection to a local machine or any other machine.
        /// generally must be used when trying to connect to another machine in the network.
        /// This function initializes the ManagementScope object.
        /// If this method is not used, then default connection is made to 'localhost'
        /// </summary>
        /// <param name="machineFullName"></param>
        /// <returns></returns>
        IProcessWMIQueryFluent ConnectTo(string machineFullName);

        /// <summary>
        /// Used to set the relative path to WMI Metabase data.
        /// The path may or may not include the machine name.
        /// Default is the 'root\Cimv2'
        /// </summary>
        /// <param name="relativePath"></param>
        /// <returns></returns>
        IProcessWMIQueryFluent UseRelativePath(string relativePath);

        /// <summary>
        /// Refine query using class name used in SelectQuery
        /// </summary>
        /// <param name="className"></param>
        /// <returns></returns>
        IProcessWMIQueryFluent UsingClass(string className);

        /// <summary>
        /// Filter SelectQuery
        /// </summary>
        /// <param name="whereClause"></param>
        /// <returns></returns>
        IProcessWMIQueryFluent WhereCondition(string whereClause);

        /// <summary>
        /// Connect using option of username and password
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        IProcessWMIQueryFluent ConnectWithOptions(string username, string password);

        /// <summary>
        /// Connect using option of username and password and use a timeout
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IProcessWMIQueryFluent ConnectWithOptions(string username, string password, TimeSpan timeout);
    }
}
