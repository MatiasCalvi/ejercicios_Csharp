using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBDAuthor
    {
        Task<List<Author>> GetAllAsync();
        Task<Author> GetAuthorByNameAsync(string pName);
        Task<Author?> GetAuthorByIDAsync(int pId);
        Task<AuthorCreate> CreateNewAuthorAsync(string pAuthorCurrent);
        Task<bool> UpdateAuthorAsync(int pId, AuthorUpdateOut pCurrentAuthor);
        Task<bool> DisableAuthorAsync(int pAuthorId);
        Task DeletedAuthorAsync(int pId);
    }
}
