namespace Clases.Exceptions
{
    public class NoHayStockException : Exception
{
    public NoHayStockException(string pCliente, string pProducto) : base($"No se realizo la compra, {pProducto} no está en stock.")
    {
        Cliente = pCliente;
        Producto = pProducto;
    }

    public string Cliente { get; }
    public string Producto { get; }
}
}
