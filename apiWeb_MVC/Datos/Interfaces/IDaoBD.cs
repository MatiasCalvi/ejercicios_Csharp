using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBD
    {
        List<UserOutput> GetAllUsers();
        UserOutput GetUserByID(int pId);
        UserUpdate? GetUserByIDU(int pId);
        UserOutput CreateNewUser(UserInput pUserInput);
        void DeletedUser(int pId);
        bool UpdateUser(int id, UserUpdate pCurrentUser);
    }
}
