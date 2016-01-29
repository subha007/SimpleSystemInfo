namespace WinSysInfo.WSIException
{
    /// <summary>
    /// Get the genral exception object
    /// </summary>
    public class ExFactoryGeneric
    {
        /// <summary>
        /// Get a new instance of exception class
        /// </summary>
        /// <returns></returns>
        public static void UnreachableCode()
        {
            throw new CustomExceptionMessage();
        }
    }
}
