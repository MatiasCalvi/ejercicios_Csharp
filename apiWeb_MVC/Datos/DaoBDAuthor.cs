using Configuracion;
using Dapper;
using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace Datos
{
    public class DaoBDAuthor : IDaoBDAuthor 
    {
        private readonly string connectionString;
        private const string getAllAuthorsQuery = "SELECT * FROM authors";
        private const string getBookByAuthorNameQuery = "SELECT * FROM authors where author_Name = @author_Name";
        private const string createBookAuthorQuery = "INSERT INTO authors(Author_Name) VALUES(@Author_Name); SELECT* FROM authors WHERE author_ID = LAST_INSERT_ID()";
        private const string getAuthorByIDQuery = "SELECT * FROM authors where Author_ID = @Author_ID";

        public DaoBDAuthor(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public List<Author> GetAll()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<Author>(getAllAuthorsQuery).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all authors.", ex);
            }
        }

        public Author GetAuthorByName(string pName)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<Author>(getBookByAuthorNameQuery, new { author_Name = pName }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting author by name {pName}.", ex);
            }
        }

        public Author? GetAuthorByID(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<Author>(getAuthorByIDQuery, new { Author_ID = pId }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting author with ID {pId}.", ex);
            }
        }

        public Author CreateNewAuthor(string pAuthorCurrent)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();
                    var param = new { Author_Name = pAuthorCurrent };
                    return dbConnection.QuerySingle<Author>(createBookAuthorQuery, param);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new author.", ex);
            }
        }
    }
}
