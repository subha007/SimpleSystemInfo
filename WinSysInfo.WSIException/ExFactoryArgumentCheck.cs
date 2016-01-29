namespace WinSysInfo.WSIException
{
    public class ExFactoryArgumentCheck
    {
        public static void Check(params object[] args)
        {
            if (args == null)
                throw new CustomExceptionMessage(new ExceptionModel(
                                        "Arguments list provided to check null is itself Null", 
                                        ExceptionLevel.ERROR,
                                        ExceptionType.INTERNAL));

            for(int indx = 0; indx < args.Length; ++indx)
            {
                if (args[indx] == null)
                    throw new CustomExceptionMessage(new ExceptionModel(
                                        string.Format("Argument {0} is null.", indx),
                                        ExceptionLevel.ERROR,
                                        ExceptionType.BUSINESS_LOGIC));
            }
        }
    }
}
