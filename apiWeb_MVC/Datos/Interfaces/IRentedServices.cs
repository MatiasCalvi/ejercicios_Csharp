using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IRentedServices
    {   
        List<RentedBook> GetAllRented();
        RentedBookOut GetRentByID(int pId);
        RentedBookOut CreateNewRent(BookOutput bookId, int userId);
        
    }
}
