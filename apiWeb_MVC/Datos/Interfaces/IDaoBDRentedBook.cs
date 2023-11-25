using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBDRentedBook
    {
        List<RentedBook> GetAllRented();
        RentedBookOut? GetRentByID(int pId);
        RentedBookOut CreateNewRent(RentedBook pBookInput);
    }
}
