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
        private const string createBookAuthorQuery = "INSERT INTO authors(Author_Name,Author_CreateDate) VALUES(@Author_Name,@Author_CreateDate); SELECT* FROM authors WHERE author_ID = LAST_INSERT_ID()";
        private const string getAuthorByIDQuery = "SELECT * FROM authors where Author_ID = @Author_ID";
        private const string disableAuthorQuery = "UPDATE authors SET Author_Status = 0 WHERE Author_ID = @Author_ID";
        private const string deletedAuthorQuery = "DELETE FROM authors WHERE Author_ID = @Author_ID";

        public DaoBDAuthor(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<Author>> GetAllAsync()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return (await dbConnection.QueryAsync<Author>(getAllAuthorsQuery)).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all authors.", ex);
            }
        }

        public async Task<Author> GetAuthorByNameAsync(string pName)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return (await dbConnection.QueryAsync<Author>(getBookByAuthorNameQuery, new { author_Name = pName })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting author by name {pName}.", ex);
            }
        }

        public async Task<Author?> GetAuthorByIDAsync(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return (await dbConnection.QueryAsync<Author>(getAuthorByIDQuery, new { Author_ID = pId })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting author with ID {pId}.", ex);
            }
        }

        public async Task<AuthorCreate> CreateNewAuthorAsync(string pAuthorCurrent)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                var param = new { Author_Name = pAuthorCurrent, Author_CreateDate = DateTime.Now };
                return await dbConnection.QuerySingleAsync<AuthorCreate>(createBookAuthorQuery, param);
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new author.", ex);
            }
        }

        public async Task<bool> UpdateAuthorAsync(int pId, AuthorUpdateOut pCurrentAuthor)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                List<string> updateFields = new();
                DynamicParameters parameters = new();

                if (pCurrentAuthor.Author_Name != null)
                {
                    updateFields.Add("Author_Name = @Author_Name");
                    parameters.Add("Author_Name", pCurrentAuthor.Author_Name);
                }

                if (updateFields.Count == 0) return false;

                parameters.Add("Author_ID", pId);
                updateFields.Add("Author_UpdateDate = @Author_UpdateDate");
                parameters.Add("Author_UpdateDate", pCurrentAuthor.Author_UpdateDate);

                string updateUserQuery = $"UPDATE authors SET {string.Join(", ", updateFields)} WHERE Author_ID = @Author_ID";

                int rowsAffected = await dbConnection.ExecuteAsync(updateUserQuery, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error updating author with ID {pId}.", ex);
            }
        }

        public async Task<bool> DisableAuthorAsync(int pAuthorId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();
                    int rowsAffected = await dbConnection.ExecuteAsync(disableAuthorQuery, new { Author_ID = pAuthorId });

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error disabling author with ID {pAuthorId}.", ex);
            }
        }

        public async Task DeletedAuthorAsync(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                await dbConnection.ExecuteAsync(deletedAuthorQuery, new { Author_ID = pId });
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error deleting author with ID {pId}.", ex);
            }
        }
    }
}
