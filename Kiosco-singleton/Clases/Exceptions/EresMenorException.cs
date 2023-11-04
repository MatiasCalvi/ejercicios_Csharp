namespace Clases.Exceptions
{
    public class EresMenorException : Exception
    {
        public EresMenorException(string pCliente, string pProducto) : base($"No puedes comprar {pProducto} porque eres menor de edad.")
        {
            Cliente = pCliente;
            Producto = pProducto;
        }

        public string Cliente { get; }
        public string Producto { get; }
    }
}

