using Datos.Interfaces;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Datos.Schemas
{
    public class Author : IAuthor
    {      
        public int Author_Id { get; set; }
        public string Author_Name { get; set; }
    }
    public class AutorInputUP 
    {
        [AllowNull]
        [JsonIgnore]
        public int AuthorID { get; set; }
        public string Author_Name { get; set; }
    }
    public class AuthorCreate : Author
    {
        public DateTime Author_CreateDate { get; set; }
    }
    public class AuthorUpdateOut : Author
    {
        public DateTime Author_UpdateDate { get; set; }
    }

    public class AuthorPrueba
    {
        public int Author_Id { get; set; }
        public string Author_Name { get; set; }
        public List<Book> Books { get; set; }
    }
}
