using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IBookServices
    {
        Task<List<BookOutput>> GetAllBooksAsync();
        Task<BookOutput> GetBookByIdAsync(int bookId);
        Task<BookOutput> GetBookByNameAsync(string bookName);
        Task<BookOutput> CreateNewBookWithAuthorNameAsync(BookWithAuthorID bookInput);
        Task<BookOutput> UpdateBookAsync(int bookId, BookInputUpdateAidString bookUpdate);
        Task<List<string>> GetBooksAndAuthorsAsync();
    }
}
