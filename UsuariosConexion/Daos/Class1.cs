using System.Data;
using Dapper;
using MySql.Data.MySqlClient;

namespace Daos
{
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
}