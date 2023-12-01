using Datos.Schemas;

namespace Datos.Interfaces
{
    public interface IRentedServices
    {
        Task<List<RentedBookOut>> GetAllRentedAsync();
        Task<RentedBookOut> GetRentByIDAsync(int pId);
        Task<RentedBookOut> CreateNewRentAsync(BookOutput pBookId, int pUserId);

    }
}
