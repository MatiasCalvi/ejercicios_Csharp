using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBDBook
    {
        List<BookOutput> GetAllBooks();
        BookOutput? GetBookByID(int pId);
        BookOutput CreateNewBook(BooWithAuthorIDInt pBookInput);
        BookOutput? GetBookByName(string pName);
        bool UpdateBook(int pId, BookInputUpdate pCurrentBook);
    }
}
