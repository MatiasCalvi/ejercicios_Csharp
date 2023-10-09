using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Daos
{

    public class RepositorioUsuarios : IRepositorioUsuarios
    {   

        private const string connectionString = "Server=localhost;Database=usuariosconexion;Uid=root;Pwd=12345678;";

        private readonly string queryObtUsuarios = "SELECT * FROM usuarios";
        private readonly string queryObtUsuarioId = "SELECT * FROM usuarios WHERE Usuario_Id = @Usuario_Id";
        private readonly string crearUsuario = "INSERT INTO usuarios (Usuario_Nombre, Usuario_Edad) VALUES (@Usuario_Nombre, @Usuario_Edad)";
        private readonly string actualizarUsuario = "UPDATE usuarios SET Usuario_Nombre = @Usuario_Nombre, Usuario_Edad = @Usuario_Edad WHERE Usuario_Id = @Usuario_Id";
        private readonly string eliminarUsuario = "DELETE FROM usuarios WHERE Usuario_Id = @Usuario_Id";

        public static IDbConnection Connection
        {
            get
            {
                return new MySqlConnection(connectionString);
            }
        }

        public List<Usuario> ListarTodosLosUsuarios()
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            return dbConnection.Query<Usuario>(queryObtUsuarios).ToList();
        }


        public Usuario? ObtenerInformacionDeUnUsuario(int id) //---> posible nulidad
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            return dbConnection.Query<Usuario>(queryObtUsuarioId, new { Usuario_Id = id }).FirstOrDefault();
        }


        public void MostrarInformacionDeUsuario(int id)
        {
            if (ObtenerInformacionDeUnUsuario(id) is Usuario usuarioInfo)
            {
                Console.WriteLine($"ID: {usuarioInfo.Usuario_Id}, Nombre: {usuarioInfo.Usuario_Nombre}, Edad: {usuarioInfo.Usuario_Edad}");
            }
            else
            {
                Console.WriteLine("No se encontró el usuario.");
            }
        }

        public void CrearUsuarioEnBD(Usuario usuario)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            dbConnection.Execute(crearUsuario, usuario);
            Console.WriteLine("Usuario creado con éxito.");
        }


        public void ActualizarUsuarioEnBD(Usuario usuario)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            dbConnection.Execute(actualizarUsuario, usuario);
            Console.WriteLine("Usuario actualizado con éxito.");
        }

        public void EliminarUsuario(int id)
        {
            using IDbConnection dbConnection = Connection;
            dbConnection.Open();
            int rowsAffected = dbConnection.Execute(eliminarUsuario, new { Usuario_Id = id });

            if (rowsAffected > 0)
            {
                Console.WriteLine("Usuario eliminado con éxito.");
            }
            else
            {
                Console.WriteLine("No se encontró el usuario o ya ha sido eliminado.");
            }
        }

    }
}
