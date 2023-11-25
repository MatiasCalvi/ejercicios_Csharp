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
    public class DaoBDBook : IDaoBDBook
    {
        private readonly string connectionString;
        private const string getAllBookQuery = "SELECT * FROM books";
        private const string getBookByIDQuery = "SELECT * FROM books WHERE book_ID = @book_ID";
        private const string getBookByNameQuery = "SELECT * FROM books WHERE book_Name = @book_Name";
        private const string createBookQuery = "INSERT INTO books(Book_Name, Book_Price, Book_CreationYear,Book_AuthorID) VALUES(@Book_Name, @Book_Price, @Book_CreationYear, @Book_AuthorID); SELECT* FROM books WHERE Book_ID = LAST_INSERT_ID()";

        public DaoBDBook(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public List<BookOutput> GetAllBooks()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<BookOutput>(getAllBookQuery).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all books.", ex);
            }
        }

        public BookOutput? GetBookByID(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<BookOutput>(getBookByIDQuery, new { book_ID = pId }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting book with ID {pId}.", ex);
            }
        }

        public BookOutput? GetBookByName(string pName)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return dbConnection.Query<BookOutput>(getBookByNameQuery, new { book_Name = pName }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting book with Name {pName}.", ex);
            }
        }

        public BookOutput CreateNewBook(BooWithAuthorIDInt pBookInput)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();
                    return dbConnection.QuerySingle<BookOutput>(createBookQuery, pBookInput);
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new book.", ex);
            }
        }

        public bool UpdateBook(int pId, BookInputUpdate pCurrentBook)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                {
                    dbConnection.Open();

                    List<string> updateFields = new();
                    DynamicParameters parameters = new();

                    if (pCurrentBook.Book_Name != null)
                    {
                        updateFields.Add("Book_Name = @Book_Name");
                        parameters.Add("Book_Name", pCurrentBook.Book_Name);
                    }

                    if (pCurrentBook.Book_Price != null)
                    {
                        updateFields.Add("Book_Price = @Book_Price");
                        parameters.Add("Book_Price", pCurrentBook.Book_Price);
                    }

                    if(pCurrentBook.Book_CreationYear != null) 
                    {
                        updateFields.Add("Book_CreationYear = @Book_CreationYear");
                        parameters.Add("Book_CreationYear",pCurrentBook.Book_CreationYear);
                    }

                    if (pCurrentBook.Book_AuthorID != null)
                    {
                        updateFields.Add("Book_AuthorID = @Book_AuthorID");
                        parameters.Add("Book_AuthorID", pCurrentBook.Book_AuthorID);
                    }

                    if (updateFields.Count == 0) return false;

                    parameters.Add("Book_ID", pId);

                    string updateUserQuery = $"UPDATE books SET {string.Join(", ", updateFields)} WHERE Book_ID = @Book_ID";

                    int rowsAffected = dbConnection.Execute(updateUserQuery, parameters);

                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error updating book with ID {pId}.", ex);
            }
        }
    }
}
