public interface IDao
{
    List<Producto> ObtenerTodosLosProductos();
    Producto ObtenerProductoPorNombre(string pNombre);
    void SumaProducto(Producto product);
    void ActualizarStock(Producto pProducto);
    void ActualizarProducto(Producto pProducto);
    void EliminarProducto(Producto pProducto);
}
