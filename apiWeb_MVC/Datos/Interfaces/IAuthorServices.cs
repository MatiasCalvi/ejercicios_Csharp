using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IAuthorServices
    {
        List<Author> GetAllUsers();
        public Author GetAuthorByName(string pName);
        public Author CreateNewAuthor(string pAuthorName);
        public Author GetInformationFromAuthor(int pId);
    }
}
