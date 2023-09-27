using System.Data;
using Dapper;
using MySql.Data.MySqlClient;
using UsuariosConexion;

public class RepositorioUsuarios
{
    private string connectionString = "Server=localhost;Database=usuariosconexion;Uid=root;Pwd=12345678;";

    public IDbConnection Connection
    {
        get
        {
            return new MySqlConnection(connectionString);
        }
    }

    public List<Usuario> ListarTodosLosUsuarios()
    {
        using (IDbConnection dbConnection = Connection)
        {
            dbConnection.Open();
            return dbConnection.Query<Usuario>("SELECT * FROM usuarios").ToList();
        }
    }

    public Usuario ObtenerInformacionDeUnUsuario(int id)
    {
        using (IDbConnection dbConnection = Connection)
        {
            dbConnection.Open();
            return dbConnection.Query<Usuario>("SELECT * FROM usuarios WHERE Usuario_Id = @Id", new { Id = id }).FirstOrDefault();
        }
    }

}

class Program
{
    static void Main(string[] args)
    {
        RepositorioUsuarios repositorio = new RepositorioUsuarios();

        
        var usuarios = repositorio.ListarTodosLosUsuarios();
        foreach (var usuario in usuarios) //---------> Listar los usuarios  
        {
            Console.WriteLine($"ID: {usuario.Usuario_Id}, Nombre: {usuario.Usuario_Nombre}, Edad: {usuario.Usuario_Edad}");
        }

        var usuarioInfo = repositorio.ObtenerInformacionDeUnUsuario(3); //--------> Listar usuario por Id
        Console.WriteLine($"ID: {usuarioInfo.Usuario_Id}, Nombre: {usuarioInfo.Usuario_Nombre}, Edad: {usuarioInfo.Usuario_Edad}");
    }
}

















