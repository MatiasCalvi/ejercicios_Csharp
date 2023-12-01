using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IBookServices
    {
        List<BookOutput> GetAllBooks();
        BookOutput GetInformationFromBook(int pId);
        BookOutput CreateNewBookWithAuthorName(BookWithAuthorID pBookInput);
        BookOutput GetInformationFromBookName(string pName);
        BookOutput UpdateBook(int pId, BookInputUpdateAidString pBookUpdate);

    }
}
