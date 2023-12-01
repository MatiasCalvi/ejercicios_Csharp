using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBDAuthor
    {
        List<Author> GetAll();
        Author GetAuthorByName(string pName);
        Author? GetAuthorByID(int pId);
        Author CreateNewAuthor(string pAuthorCurrent);
    }
}
