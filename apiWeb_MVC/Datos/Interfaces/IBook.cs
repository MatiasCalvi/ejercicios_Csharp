namespace Datos.Interfaces
{
    public interface IBook
    {
        public int Book_ID { get; set; }
        public string Book_Name { get; set; }
        public int Book_Price { get; set; }
        public short Book_CreationYear { get; set; }
    }
}
