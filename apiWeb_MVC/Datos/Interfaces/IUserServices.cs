using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IUserServices
    {
        Task<UserOutput> GetInformationFromUserAsync(int pId);
        Task<UserInputUpdate> GetInformationFromUserUAsync(int pId);
        Task<List<UserOutput>> GetAllUsersAsync();
        Task<List<UserOutput>> GetUsersByIdsAsync(List<int> pUserIds);
        Task<UserInputUpdate> GetUserByEmailAsync(string pEmail);
        Task<UserOutputCreate> CreateNewUserAsync(UserInput pUserInput);
        Task<UserOutput> UpdateUserAsync(int pId, UserInputUpdate pUserUpdate);
        Task<UserOutput> VerifyUserAsync(string pEmail, string pPassword);
        Task<bool> DisableUserAsync(int pId);
        Task DeletedUserAsync(int pId);
    }
}
