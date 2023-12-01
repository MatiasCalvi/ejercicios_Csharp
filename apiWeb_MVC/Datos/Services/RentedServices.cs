using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;

namespace Datos.Services
{
    public class RentedServices : IRentedServices
    {
        private IDaoBDRentedBook _daoBDRentedBook;

        public RentedServices(IDaoBDRentedBook daoBDRentedBook)
        {
            _daoBDRentedBook = daoBDRentedBook;
        }

        public async Task<List<RentedBookOut>> GetAllRentedAsync()
        {
            return await _daoBDRentedBook.GetAllRentedAsync();
        }

        public async Task<RentedBookOut> GetRentByIDAsync(int pId)
        {
            try
            {
                RentedBookOut rent = await _daoBDRentedBook.GetRentByIDAsync(pId);
                if (rent == null)
                {
                    throw new NotFoundException($"Rent with ID {pId} was not found in the database.");
                }
                return rent;
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"An error occurred while getting rent with ID {pId}.", ex);
            }
        }

        public async Task<RentedBookOut> CreateNewRentAsync(BookOutput pBookId, int pUserId)
        {
            try
            {
                DateTime rentalStartDate = DateTime.Now;
                DateTime rentalDueDate = rentalStartDate.AddDays(5);

                RentedBook newRent = new RentedBook
                {
                    RB_BookID = pBookId.Book_ID,
                    RB_UserID = pUserId,
                    RB_RentalStartDate = rentalStartDate,
                    RB_RentDueDate = rentalDueDate,
                    RB_Price = pBookId.Book_Price
                };

                return await _daoBDRentedBook.CreateNewRentAsync(newRent);
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new rent.", ex);
            }
        }
    }
}
