namespace Datos.Interfaces
{
    public interface IRentedBook
    {
        public int RB_Id { get; set; }
        public int RB_BookID { get; set; }
        public int RB_UserID { get; set; }
        public DateTime RB_RentalStartDate { get; set; }
        public DateTime RB_RentDueDate { get; set; }
        public int RB_Price { get; set; }
    }
}
