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

        public List<RentedBook> GetAllRented()
        {
            return _daoBDRentedBook.GetAllRented();
        }

        public RentedBookOut GetRentByID(int pId)
        {
            try
            {
                RentedBookOut rent = _daoBDRentedBook.GetRentByID(pId);
                return rent;
            }
            catch (Exception ex)
            {
                throw new NotFoundException($"Rent with ID {pId} was not found in the database.", ex);
            }
        }

        public RentedBookOut CreateNewRent(BookOutput pBookId, int pUserId)
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

                return _daoBDRentedBook.CreateNewRent(newRent);
            }
            catch (Exception ex)
            {
                throw new DatabaseTransactionException("Error creating a new rent.", ex);
            }
        }
    }
}
