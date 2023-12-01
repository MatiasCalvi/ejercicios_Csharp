using Datos.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Datos.Schemas
{
    public class RentedBook : IRentedBook
    {
        [AllowNull]
        [JsonIgnore]
        public int RB_Id {  get; set; }
        public int RB_BookID { get; set; }
        public int RB_UserID { get; set; }
        public DateTime RB_RentalStartDate { get; set; }
        public DateTime RB_RentDueDate { get; set; }
        public int RB_Price { get; set; }
        public RentedBook() { }
    }
    public class RentedBookOut : IRentedBook
    {
        public int RB_Id { get; set; }
        public int RB_BookID { get; set; }
        public int RB_UserID { get; set; }
        public DateTime RB_RentalStartDate { get; set; }
        public DateTime RB_RentDueDate { get; set; }
        public int RB_Price { get; set; }
        public RentedBookOut() { }
    }
}