using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IUserServices
    {
        UserOutput GetInformationFromUser(int pId);
        List<UserOutput> GetAllUsers();
        List<UserOutput> GetUsersByIds(List<int> userIds);
        bool VerifyPassword(string userInput, string hashedPassword);
        UserOutput CreateNewUser(UserInput userInput);
        void DeletedUser(int id);
    }
}
