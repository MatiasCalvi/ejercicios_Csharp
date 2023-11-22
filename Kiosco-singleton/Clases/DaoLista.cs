using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

public class DaoLista : IDao
{
    private const string connectionString = "Server=localhost;Database=productoskioscobd;Uid=root;Pwd=12345678;";
    private readonly string listaCompletaString = "SELECT * FROM productos";
    private readonly string productoPorNombreString = "SELECT * FROM productos WHERE producto_Nombre = @nombre";
    private readonly string productoPorIdString = "SELECT * FROM productos WHERE producto_ID = @producto_ID";
    private readonly string sumaProductoString = "INSERT INTO productos (producto_Nombre, producto_Precio, producto_Stock, producto_EsAlcohol, producto_RequiereEdad) VALUES (@Nombre, @Precio, @Stock, @esAlcohol, @RequiereEdad)";
    private readonly string actualizarProductoString = "UPDATE productos SET producto_Nombre = @Nombre, producto_Precio = @Precio, producto_Stock = @Stock, producto_EsAlcohol = @EsAlcohol, producto_RequiereEdad = @RequiereEdad WHERE producto_ID = @producto_ID";
    private readonly string actualizarStockString = "UPDATE productos SET producto_Stock = @Stock WHERE producto_Nombre = @Nombre";
    private readonly string eliminarProductoString = "DELETE FROM productos WHERE producto_Nombre = @Nombre";
    public static IDbConnection Connection
    {
        get
        {
            return new MySqlConnection(connectionString);
        }
    }

    public List<Producto> ObtenerTodosLosProductos()
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();
        return dbConnection.Query<Producto>(listaCompletaString).ToList();
    }

    public Producto ObtenerProductoPorNombre(string pNombre)
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();
        return dbConnection.QueryFirstOrDefault<Producto>(productoPorNombreString, new { nombre = pNombre });
    }

    public Producto? ObtenerInformacionDeUnProducto(int id)
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();
        return dbConnection.Query<Producto>(productoPorIdString, new { producto_ID = id }).FirstOrDefault();
    }

    public void SumaProducto(Producto pProducto)
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();
        dbConnection.Execute(sumaProductoString, pProducto);
    }

    public void ActualizarProducto(Producto pProducto)
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();

        var parameters = new DynamicParameters();

        parameters.Add("@producto_ID", pProducto.producto_ID);
        parameters.Add("@Nombre", pProducto.producto_Nombre);
        parameters.Add("@Precio", pProducto.producto_Precio);
        parameters.Add("@Stock", pProducto.producto_Stock);
        parameters.Add("@EsAlcohol", pProducto.producto_EsAlcohol);
        parameters.Add("@RequiereEdad", pProducto.producto_RequiereEdad);

        dbConnection.Execute(actualizarProductoString, parameters);
    }


    public void ActualizarStock(Producto pProducto)
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();
        
        var parameters = new DynamicParameters();
        
        parameters.Add("@Nombre", pProducto.producto_Nombre);
        parameters.Add("@Stock", pProducto.producto_Stock);
        
        dbConnection.Execute(actualizarStockString, parameters);
    }



    public void EliminarProducto(Producto pProducto)
    {
        using IDbConnection dbConnection = Connection;
        dbConnection.Open();
        dbConnection.Execute(eliminarProductoString, pProducto);
    }
}
