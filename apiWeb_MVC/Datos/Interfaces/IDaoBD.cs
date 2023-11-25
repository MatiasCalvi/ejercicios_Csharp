using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBD
    {
        List<UserOutput> GetAllUsers();
        UserOutput GetUserByID(int pId);
        UserInputUpdate? GetUserByIDU(int pId);
        UserInputUpdate GetUserByEmail(string email);
        UserOutputCreate CreateNewUser(UserInput pUserInput);
        bool UpdateUser(int pId, UserInputUpdate pCurrentUser);
        bool DisableUser(int pUserId);
        void DeletedUser(int pId);
    }
}
