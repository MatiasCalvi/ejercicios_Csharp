using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Daos
{
        public class RepositorioUsuarios
        {
            private string connectionString = "Server=localhost;Database=usuariosconexion;Uid=root;Pwd=12345678;";
            
            private string queryObtUsuarios = "SELECT * FROM usuarios";
            private string queryObtUsuarioId = "SELECT * FROM usuarios WHERE Usuario_Id = @Usuario_Id";
            private string crearUsuario = "INSERT INTO usuarios (Usuario_Nombre, Usuario_Edad) VALUES (@Usuario_Nombre, @Usuario_Edad)";
            private string actualizarUsuario = "UPDATE usuarios SET Usuario_Nombre = @Usuario_Nombre, Usuario_Edad = @Usuario_Edad WHERE Usuario_Id = @Usuario_Id";
            private string eliminarUsuario = "DELETE FROM usuarios WHERE Usuario_Id = @Usuario_Id";

            public IDbConnection Connection
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

            public void CrearUsuario(Usuario usuario)
            {
                using IDbConnection dbConnection = Connection;
                dbConnection.Open();
                dbConnection.Execute(crearUsuario, usuario);
                Console.WriteLine("Usuario creado con éxito.");
            }

            public void ActualizarUsuario(Usuario usuario)
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
                dbConnection.Execute(eliminarUsuario, new { Usuario_Id = id });
                Console.WriteLine("Usuario eliminado con éxito.");
            }


    }
}