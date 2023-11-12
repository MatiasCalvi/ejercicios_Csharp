using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBD
    {
        List<UserOutput> GetAllUsers();
        UserOutput GetUserByID(int pId);
        UserInputUpdate? GetUserByIDU(int pId);
        UserOutputCreate CreateNewUser(UserInput pUserInput);
        bool UpdateUser(int id, UserInputUpdate pCurrentUser);
        public bool DisableUser(int pUserId);
        void DeletedUser(int pId);
    }
}
