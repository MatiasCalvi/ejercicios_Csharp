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
        private const string pruebaLibroAutores = "SELECT b.Book_ID, b.Book_Name, b.Book_Price, b.Book_CreationYear, b.Book_AuthorID,a.Author_Id, a.Author_Name FROM books b INNER JOIN authors a ON b.Book_AuthorID = a.Author_Id";

        public DaoBDBook(IOptions<BDConfiguration> dbConfig)
        {
            connectionString = dbConfig.Value.ConnectionString;
        }

        private IDbConnection CreateConnection()
        {
            IDbConnection dbConnection = new MySqlConnection(connectionString);
            return dbConnection;
        }

        public async Task<List<BookOutput>> GetAllBooksAsync()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return (await dbConnection.QueryAsync<BookOutput>(getAllBookQuery)).ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all books.", ex);
            }
        }

        public async Task<BookOutput?> GetBookByIDAsync(int pId)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<BookOutput>(getBookByIDQuery, new { book_ID = pId });
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting book with ID {pId}.", ex);
            }
        }

        public async Task<BookOutput?> GetBookByNameAsync(string pName)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return await dbConnection.QueryFirstOrDefaultAsync<BookOutput>(getBookByNameQuery, new { book_Name = pName });
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting book with Name {pName}.", ex);
            }
        }

        public async Task<BookOutput> CreateNewBookAsync(BooWithAuthorIDInt pBookInput)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();
                return await dbConnection.QuerySingleAsync<BookOutput>(createBookQuery, pBookInput);
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new book.", ex);
            }
        }

        public async Task<bool> UpdateBookAsync(int pId, BookInputUpdate pCurrentBook)
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                List<string> updateFields = new List<string>();
                DynamicParameters parameters = new DynamicParameters();

                if (!string.IsNullOrEmpty(pCurrentBook.Book_Name))
                {
                    updateFields.Add("Book_Name = @Book_Name");
                    parameters.Add("Book_Name", pCurrentBook.Book_Name);
                }

                if (pCurrentBook.Book_Price.HasValue)
                {
                    updateFields.Add("Book_Price = @Book_Price");
                    parameters.Add("Book_Price", pCurrentBook.Book_Price);
                }

                if (pCurrentBook.Book_CreationYear.HasValue)
                {
                    updateFields.Add("Book_CreationYear = @Book_CreationYear");
                    parameters.Add("Book_CreationYear", pCurrentBook.Book_CreationYear);
                }

                if (pCurrentBook.Book_AuthorID.HasValue)
                {
                    updateFields.Add("Book_AuthorID = @Book_AuthorID");
                    parameters.Add("Book_AuthorID", pCurrentBook.Book_AuthorID);
                }

                if (updateFields.Count == 0) return false;

                parameters.Add("Book_ID", pId);

                string updateBookQuery = $"UPDATE books SET {string.Join(", ", updateFields)} WHERE Book_ID = @Book_ID";

                int rowsAffected = await dbConnection.ExecuteAsync(updateBookQuery, parameters);

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException($"Error updating book with ID {pId}.", ex);
            }
        }

        public async Task<List<Book>> GetBooksAndAuthorsAsync()
        {
            try
            {
                using IDbConnection dbConnection = CreateConnection();
                dbConnection.Open();

                var result = await dbConnection.QueryAsync<Book, AuthorPrueba, Book>(
                    pruebaLibroAutores,
                    (book, author) =>
                    {
                        book.Author = author;
                        return book;
                    },
                    splitOn: "Author_Id"
                );

                return result.ToList();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get books and authors.", ex);
            }
        }
    }
}
