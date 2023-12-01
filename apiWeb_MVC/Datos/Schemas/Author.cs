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
}
