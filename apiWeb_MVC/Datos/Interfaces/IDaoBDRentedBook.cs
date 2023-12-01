using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IDaoBDRentedBook
    {
        Task<List<RentedBookOut>> GetAllRentedAsync();
        Task<RentedBookOut?> GetRentByIDAsync(int pId);
        Task<RentedBookOut> CreateNewRentAsync(RentedBook pBookInput);
    }
}
