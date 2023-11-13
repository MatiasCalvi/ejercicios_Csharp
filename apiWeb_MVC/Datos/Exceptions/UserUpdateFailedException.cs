namespace Datos.Exceptions
{    
    public class UserUpdateFailedException : Exception
    {   
        public UserUpdateFailedException() { }
        public UserUpdateFailedException(string message) : base(message) { }
        public UserUpdateFailedException(string message, Exception innerException) : base(message, innerException) { }
    }
}