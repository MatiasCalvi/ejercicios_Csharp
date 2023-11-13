namespace Datos.Exceptions
{
    public class UserDeletionFailedException : Exception
    {
        public UserDeletionFailedException() { }
        public UserDeletionFailedException(string message) : base(message) { }
        public UserDeletionFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
