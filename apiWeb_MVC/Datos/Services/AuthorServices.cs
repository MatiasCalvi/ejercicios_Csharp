using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using Datos.Validate;

namespace Datos.Services
{
    public class AuthorServices : IAuthorServices
    {
        private IDaoBDAuthor _daoBDAuthor;
        public AuthorServices(IDaoBDAuthor daoBDAuthor)
        {
            _daoBDAuthor = daoBDAuthor;
        }

        public async Task<List<Author>> GetAllAuthorsAsync()
        {
            return await _daoBDAuthor.GetAllAsync();
        }

        public async Task<Author> GetAuthorByNameAsync(string pName)
        {
            return await _daoBDAuthor.GetAuthorByNameAsync(pName);
        }

        public async Task<Author> GetInformationFromAuthorAsync(int pId)
        {
            try
            {
                Author author = await _daoBDAuthor.GetAuthorByIDAsync(pId);
                return author;
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"Author with ID {pId} was not found in the database.", ex);
            }
        }

        public async Task<AuthorCreate> CreateNewAuthorAsync(string pAuthorCurrent)
        {
            try
            {
                AuthorCreate author = await _daoBDAuthor.CreateNewAuthorAsync(pAuthorCurrent);

                if (author == null)
                {
                    throw new CreationFailedException("Failed to create a new author.");
                }

                return author;
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new author.", ex);
            }
        }

        public async Task<AuthorUpdateOut> UpdateAuthorAsync(int pId, AutorInputUP pUserUpdate)
        {
            try
            {
                Author currentAuthor = await GetInformationFromAuthorAsync(pId);

                if (currentAuthor == null)
                {
                    throw new NotFoundException($"The author with ID {pId} was not found in the database.");
                }

                DateTime UpdateDate = DateTime.Now;

                AuthorUpdateOut authorUpdateOut = new AuthorUpdateOut{
                    Author_Id = pId,
                    Author_Name = currentAuthor.Author_Name,
                    Author_UpdateDate = DateTime.Now,
                };

                authorUpdateOut.Author_Name = pUserUpdate.Author_Name ?? authorUpdateOut.Author_Name;
                authorUpdateOut.Author_UpdateDate = UpdateDate;

                bool updated = await _daoBDAuthor.UpdateAuthorAsync(pId, authorUpdateOut);

                if (updated)
                {
                    Author author = await _daoBDAuthor.GetAuthorByIDAsync(pId);
                    AuthorUpdateOut authorWithDate = new AuthorUpdateOut
                    {
                        Author_Id = author.Author_Id,
                        Author_Name = author.Author_Name,
                        Author_UpdateDate = UpdateDate
                    };

                    return authorWithDate;
                }
                else
                {
                    throw new UpdateFailedException($"Failed to update the author with ID {pId}.");
                }
            }
            catch (Exception ex)
            {
                throw new UpdateFailedException($"Error occurred while updating the author with ID {pId}.", ex);
            }
        }

        public async Task<bool> DisableAuthorAsync(int pId)
        {
            bool result = await _daoBDAuthor.DisableAuthorAsync(pId);

            if (!result)
            {
                throw new DeletionFailedException($"Failed to disable the author with ID {pId}.");
            }

            return result;
        }

        public async Task DeletedAuthorAsync(int pId)
        {
            try
            {
                await _daoBDAuthor.DeletedAuthorAsync(pId);
            }
            catch (Exception ex)
            {
                throw new DeletionFailedException($"Error occurred while deleting the author with ID {pId}.", ex);
            }
        }
    }
}
