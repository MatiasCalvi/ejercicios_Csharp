using Clases.Interfaces;

namespace Clases.Exceptions
{
    public class VedaElectoralException : Exception
    {
        public VedaElectoralException(string pCliente, string pProducto) : base($"No puedes comprar {pProducto} debido a la veda electoral.")
        {
            Cliente = pCliente;
            Producto = pProducto;
        }

        public string Cliente { get; }
        public string Producto { get; }
    }
}
