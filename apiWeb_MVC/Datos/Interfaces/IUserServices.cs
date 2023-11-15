using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IUserServices
    {
        UserOutput GetInformationFromUser(int pId);
        UserInputUpdate GetInformationFromUserU(int pId);
        List<UserOutput> GetAllUsers();
        List<UserOutput> GetUsersByIds(List<int> userIds);
        UserInputUpdate GetUserByEmail(string email);
        UserOutputCreate CreateNewUser(UserInput userInput);
        public bool DisableUser(int pId);
        void DeletedUser(int id);
    }
}
