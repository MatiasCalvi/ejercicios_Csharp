using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBDBook
    {
        Task<List<BookOutput>> GetAllBooksAsync();
        Task<BookOutput?> GetBookByIDAsync(int pId);
        Task<BookOutput?> GetBookByNameAsync(string pName);
        Task<BookOutput> CreateNewBookAsync(BooWithAuthorIDInt pBookInput);
        Task<bool> UpdateBookAsync(int pId, BookInputUpdate pCurrentBook);
        Task<List<Book>> GetBooksAndAuthorsAsync();
    }
}
