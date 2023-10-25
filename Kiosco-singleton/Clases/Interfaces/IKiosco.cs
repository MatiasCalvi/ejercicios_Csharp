namespace Clases.Interfaces
{
    public interface IKiosco
    {
        public void MostrarProductos();
        public List<IProducto> Productos();
        public Producto BuscarProducto(string pNombre);
    }
}
