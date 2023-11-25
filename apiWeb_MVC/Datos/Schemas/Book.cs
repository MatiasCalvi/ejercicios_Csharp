using Datos.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Datos.Schemas
{
    public class BookOutput : IBook 
    {
        public int Book_ID { get; set; }
        public string Book_Name { get; set; }
        public int Book_Price { get; set; }
        public short Book_CreationYear { get; set; }
        public int Book_AuthorID { get; set; }
        public BookOutput() { }
    }

    public class BookInput : IBook
    {
        [AllowNull]
        [JsonIgnore]
        public int Book_ID { get; set; }

        [Required(ErrorMessage = "The book name is required.")]
        public string Book_Name { get; set; }

        [Required(ErrorMessage = "The price is required.")]
        public int Book_Price { get; set; }

        [Required(ErrorMessage = "The creation year is required.")]
        [Range(1901, 2155, ErrorMessage = "The year must be between 1901 and 2155")]
        public short Book_CreationYear { get; set; }
        public BookInput() { }
    }

    public class BookInputUpdate
    {
        [AllowNull]
        [JsonIgnore]
        public int Book_ID { get; set; }
        
        [AllowNull]
        [MinLength(1, ErrorMessage = "The author name cannot be empty.")]
        public string? Book_Name { get; set; }
        
        [AllowNull]
        public int? Book_Price { get; set; }
        
        [AllowNull]
        [Range(1901, 2155, ErrorMessage = "The year must be between 1901 and 2155")]
        public short? Book_CreationYear { get; set; }
        
        [AllowNull]
        public int? Book_AuthorID { get; set; }
        public BookInputUpdate() { }
    }

    public class BookInputUpdateAidString
    {
        [AllowNull]
        [JsonIgnore]
        public int Book_ID { get; set; }

        [AllowNull]
        public string? Book_Name { get; set; }

        [AllowNull]
        public int? Book_Price { get; set; }

        [AllowNull]
        [Range(1901, 2023, ErrorMessage = "The year must be between 1901 and 2023")]
        public short? Book_CreationYear { get; set; }

        [AllowNull]
        [MinLength(1, ErrorMessage = "The author name cannot be empty.")]
        public string? Book_AuthorID { get; set; }
        public BookInputUpdateAidString() { }
    }

    public class BookWithAuthorID : BookInput
    {
        public string Book_AuthorID { get; set; }
    }

    public class BooWithAuthorIDInt : BookInput
    {
        public int Book_AuthorID { set; get; }
    }

}
