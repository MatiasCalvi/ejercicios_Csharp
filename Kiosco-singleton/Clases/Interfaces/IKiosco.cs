namespace Clases.Interfaces
{
    public interface IKiosco
    {
        public void MostrarProductos();
        public List<Producto> ListaProductos();
        public Producto BuscarProducto(string pNombre);
    }
}
