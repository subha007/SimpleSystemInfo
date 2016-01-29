namespace WinSysInfo.WSIException
{
    public class ExceptionModel
    {
        public ExceptionLevel Level { get; set; }
        public ExceptionType EType { get; set; }
        public string Message { get; set; }

        public ExceptionModel() : this("No Message Provided", ExceptionLevel.ERROR, ExceptionType.INTERNAL) { }
        public ExceptionModel(string message) : this(message, ExceptionLevel.ERROR, ExceptionType.BUSINESS_LOGIC) { }
        public ExceptionModel(string message, ExceptionLevel level) : this(message, level, ExceptionType.BUSINESS_LOGIC) { }
        public ExceptionModel(string message, ExceptionLevel level, ExceptionType eType)
        {
            this.Level = level;
            this.EType = eType;
            this.Message = message;
        }
    }
}
