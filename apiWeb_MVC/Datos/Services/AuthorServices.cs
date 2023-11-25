using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;

namespace Datos.Services
{
    public class AuthorServices : IAuthorServices
    {
        private IDaoBDAuthor _daoBDAuthor;
        public AuthorServices(IDaoBDAuthor daoBDAuthor)
        {
            _daoBDAuthor = daoBDAuthor;
        }

        public List<Author> GetAllUsers()
        {
            return _daoBDAuthor.GetAll();
        }
        public Author GetAuthorByName(string pName) 
        {
            Author author = _daoBDAuthor.GetAuthorByName(pName);
            return author;
        }

        public Author GetInformationFromAuthor(int pId)
        {
            try
            {
                Author author = _daoBDAuthor.GetAuthorByID(pId);
                return author;
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"User with ID {pId} was not found in the database.", ex);
            }
        }

        public Author CreateNewAuthor(string pAuthorCurrent) 
        {
            try
            {   
                Author author = _daoBDAuthor.CreateNewAuthor(pAuthorCurrent);

                if(author == null)
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
    }
}
