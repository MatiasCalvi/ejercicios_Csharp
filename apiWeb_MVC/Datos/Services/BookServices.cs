using Datos.Interfaces;
using Datos.Schemas;
using Datos.Exceptions;

namespace Datos.Services
{
    public class BookServices : IBookServices
    {
        private IDaoBDBook _daoBDBook;
        private IAuthorServices _authorServices;

        public BookServices(IDaoBDBook daoBDBook, IAuthorServices authorServices)
        {
            _daoBDBook = daoBDBook;
            _authorServices = authorServices;
        }

        private BooWithAuthorIDInt CreateBooWithAuthorIDInt(BookWithAuthorID pBookInput, int authorId) 
        { 
            return new BooWithAuthorIDInt { 
                Book_Name = pBookInput.Book_Name, 
                Book_Price = pBookInput.Book_Price, 
                Book_CreationYear = pBookInput.Book_CreationYear, 
                Book_AuthorID = authorId, }; 
        
        }
        public async Task<List<BookOutput>> GetAllBooksAsync()
        {
            try
            {
                return await _daoBDBook.GetAllBooksAsync();
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all books.", ex);
            }
        }

        public async Task<BookOutput> GetBookByIdAsync(int bookId)
        {
            try
            {
                return await _daoBDBook.GetBookByIDAsync(bookId) ?? throw new NotFoundException($"Book with ID {bookId} was not found in the database.");
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting book with ID {bookId}.", ex);
            }
        }

        public async Task<BookOutput> GetBookByNameAsync(string bookName)
        {
            try
            {
                return await _daoBDBook.GetBookByNameAsync(bookName) ?? throw new NotFoundException($"Book with name {bookName} was not found in the database.");
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException($"Error getting book with name {bookName}.", ex);
            }
        }

        public async Task<BookOutput> CreateNewBookWithAuthorNameAsync(BookWithAuthorID bookInput)
        {
            try
            {
                Author author = await _authorServices.GetAuthorByNameAsync(bookInput.Book_AuthorID);
                int authorId = (author == null) ? (await _authorServices.CreateNewAuthorAsync(bookInput.Book_AuthorID)).Author_Id : author.Author_Id;

                var bookWithIdInt = CreateBooWithAuthorIDInt(bookInput, authorId);
                return await CreateNewBookAsync(bookWithIdInt);
            }
            catch (Exception ex)
            {
                throw new CreationFailedException("An error occurred while creating a new book with the author's name.", ex);
            }
        }

        private async Task<BookOutput> CreateNewBookAsync(BooWithAuthorIDInt bookInput)
        {
            try
            {
                BookOutput bookOutput = await _daoBDBook.CreateNewBookAsync(bookInput) ?? throw new CreationFailedException("Failed to create a new book.");

                return bookOutput;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error occurred while creating a new book.", ex);
            }
        }

        public async Task<BookOutput> UpdateBookAsync(int bookId, BookInputUpdateAidString bookUpdate)
        {
            try
            {
                BookOutput currentBook = await GetBookByIdAsync(bookId) ?? throw new NotFoundException($"The book with ID {bookId} was not found in the database.");

                Author author = null;

                if (bookUpdate.Book_AuthorID != null)
                {
                    author = await _authorServices.GetAuthorByNameAsync(bookUpdate.Book_AuthorID) ?? throw new NotFoundException($"The author was not found in the database.");
                }

                currentBook.Book_Name = bookUpdate.Book_Name ?? currentBook.Book_Name;
                currentBook.Book_Price = bookUpdate.Book_Price ?? currentBook.Book_Price;
                currentBook.Book_CreationYear = bookUpdate.Book_CreationYear ?? currentBook.Book_CreationYear;

                if (author != null && author.Author_Id != currentBook.Book_AuthorID)
                {
                    currentBook.Book_AuthorID = author.Author_Id;
                }

                BookInputUpdate bookInputUpdate = new BookInputUpdate
                {
                    Book_ID = currentBook.Book_ID,
                    Book_Name = currentBook.Book_Name,
                    Book_Price = currentBook.Book_Price,
                    Book_CreationYear = currentBook.Book_CreationYear,
                    Book_AuthorID = currentBook.Book_AuthorID
                };

                bool updated = await _daoBDBook.UpdateBookAsync(bookId, bookInputUpdate);

                return updated ? await GetBookByIdAsync(bookId) : throw new UpdateFailedException($"Failed to update the book with ID {bookId}.");
            }
            catch (Exception ex)
            {
                throw new UpdateFailedException($"Error occurred while updating the book with ID {bookId}.", ex);
            }
        }

        public async Task<List<string>> GetBooksAndAuthorsAsync()
        {
            try
            {
                var books = await _daoBDBook.GetBooksAndAuthorsAsync();

                var results = new List<string>();

                foreach (var book in books)
                {
                    results.Add($"Book: {book.Book_Name}, Author: {book.Author.Author_Name}");
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<string>(); 
            }
        }
    }
}
