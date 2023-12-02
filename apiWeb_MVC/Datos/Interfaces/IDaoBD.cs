using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBD
    {
        Task<List<UserOutput>> GetAllUsersAsync();
        Task<UserOutput?> GetUserByIDAsync(int pId);
        Task<UserInputUpdate?> GetUserByIDUAsync(int pId);
        Task<UserInputUpdate> GetUserByEmailAsync(string pEmail);
        Task<UserOutputCreate> CreateNewUserAsync(UserInput pUserInput);
        Task<bool> UpdateUserAsync(int pId, UserInputUpdate pCurrentUser);
        Task<bool> DisableUserAsync(int pUserId);
        Task DeletedUserAsync(int pId);
    }
}
