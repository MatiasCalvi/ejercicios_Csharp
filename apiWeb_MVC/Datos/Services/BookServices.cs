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
        public List<BookOutput> GetAllBooks()
        {
            return _daoBDBook.GetAllBooks();
        }

        public BookOutput GetInformationFromBook(int pId)
        {
            try
            {
                BookOutput book = _daoBDBook.GetBookByID(pId);
                return book;
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"Book with ID {pId} was not found in the database.", ex);
            }
        }

        public BookOutput GetInformationFromBookName(string pName)
        {
            try
            {
                BookOutput book = _daoBDBook.GetBookByName(pName);
                return book;
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"Book with {pName} was not found in the database.", ex);
            }
        }

        public BookOutput CreateNewBookWithAuthorName(BookWithAuthorID pBookInput)
        {
            try
            {
                Author author = _authorServices.GetAuthorByName(pBookInput.Book_AuthorID); int authorId;

                if (author == null)
                {
                    author = _authorServices.CreateNewAuthor(pBookInput.Book_AuthorID); 
                    authorId = author.Author_Id;
                }
                else
                {
                    authorId = author.Author_Id;
                }
                var bookWithIdInt = CreateBooWithAuthorIDInt(pBookInput, authorId);
                return CreateNewBook(bookWithIdInt);
            }
            catch (Exception ex)
            {
                Console.WriteLine(pBookInput.Book_CreationYear);
                throw new CreationFailedException("An error occurred while creating a new book with the author's name.", ex);
            }

        }

        internal BookOutput CreateNewBook(BooWithAuthorIDInt pBookInput)
        {
            try
            {
                BookOutput bookOutput = _daoBDBook.CreateNewBook(pBookInput);

                if (bookOutput == null)
                {
                    throw new CreationFailedException("Failed to create a new book.");
                }

                return bookOutput;
            }
            catch (Exception ex)
            {
                throw new CreationFailedException("Error occurred while creating a new book.", ex);
            }

        }

        public BookOutput UpdateBook(int pId, BookInputUpdateAidString pBookUpdate)
        {
            try
            {
                BookOutput currentBook = GetInformationFromBook(pId);
                Author author = null;

                if (currentBook == null)
                {
                    throw new NotFoundException($"The book with ID {pId} was not found in the database.");
                }

                if (pBookUpdate.Book_AuthorID != null)
                {
                    author = _authorServices.GetAuthorByName(pBookUpdate.Book_AuthorID);

                    if (author == null)
                    {
                        throw new NotFoundException($"The author was not found in the database.");
                    }
                }

                currentBook.Book_Name = pBookUpdate.Book_Name ?? currentBook.Book_Name;
                currentBook.Book_Price = pBookUpdate.Book_Price ?? currentBook.Book_Price;
                currentBook.Book_CreationYear = pBookUpdate.Book_CreationYear ?? currentBook.Book_CreationYear;

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
                    Book_AuthorID = currentBook.Book_AuthorID,
                };

                bool updated = _daoBDBook.UpdateBook(pId, bookInputUpdate);

                if (updated)
                {
                    BookOutput book = _daoBDBook.GetBookByID(pId);

                    return book;
                }
                else
                {
                    throw new UpdateFailedException($"Failed to update the book with ID {pId}.");
                }
            }
            catch (Exception ex)
            {
                throw new UpdateFailedException($"Error occurred while updating the book with ID {pId}.", ex);
            }
        }
    }
}
