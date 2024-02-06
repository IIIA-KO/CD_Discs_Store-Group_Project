namespace CD_Disc_Store_React_ASP_NET_Core.Server.Utilities.Exceptions
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException() : base() { }

        public DatabaseOperationException(string message) : base(message) { }

        public DatabaseOperationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
