namespace Clases.Exceptions
{
    public class NoHayProductoException : Exception
    {
        public NoHayProductoException() : base("No tenemos dicho producto")
        {

        }

        public NoHayProductoException(string message) : base(message)
        {

        }
    }
}