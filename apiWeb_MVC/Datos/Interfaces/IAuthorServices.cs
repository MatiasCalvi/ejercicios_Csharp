using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IAuthorServices
    {
        Task<List<Author>> GetAllAuthorsAsync();
        Task<Author> GetAuthorByNameAsync(string pName);
        Task<Author> GetInformationFromAuthorAsync(int pId);
        Task<AuthorCreate> CreateNewAuthorAsync(string pAuthorCurrent);
        Task<AuthorUpdateOut> UpdateAuthorAsync(int pId, AutorInputUP pUserUpdate);
        Task<bool> DisableAuthorAsync(int pId);
        Task DeletedAuthorAsync(int pId);
    }
}
