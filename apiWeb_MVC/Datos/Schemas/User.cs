using Datos.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Datos.Schemas
{
    public class UserInput: IUser
    {
        [AllowNull]
        [JsonIgnore]
        public int User_ID { get; set; }

        [Required(ErrorMessage = "The name is required.", AllowEmptyStrings = false)]
        [MinLength(2, ErrorMessage = "The name must be at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "The name cannot be more than 50 characters.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "The name must begin with a capital letter and cannot contain numbers, hyphens, or underscores.")]
        public string User_Name { get; set; }

        [Required(ErrorMessage = "The last name is mandatory.", AllowEmptyStrings = false)]
        [MinLength(2, ErrorMessage = "The last name must be at least 2 characters.")]
        [MaxLength(50, ErrorMessage = "The last name cannot be more than 50 characters.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "The last name must begin with a capital letter and cannot contain numbers, hyphens, or underscores.")]
        public string User_LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email does not have a valid format.")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "The password cannot be more than 20 characters.")]
        public string User_Password { get; set; }

        [AllowNull]
        [JsonIgnore]
        public DateTime? User_CreationDate { get; set; }

    }
    public class UserInputUpdate : IUser
    {
        [AllowNull]
        public int User_ID { get; set; }

        [AllowNull]
        [MinLength(2,ErrorMessage = "The name must be at least 2 characters.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "The name must begin with a capital letter and cannot contain numbers, hyphens, or underscores.")]
        [MaxLength(50, ErrorMessage = "The name cannot be more than 50 characters.")]
        public string? User_Name { get; set; }

        [AllowNull]
        [MinLength(2, ErrorMessage = "The last name must be at least 2 characters.")]
        [RegularExpression("^[A-Z][a-z]+", ErrorMessage = "The last name must begin with a capital letter and cannot contain numbers, hyphens, or underscores.")]
        [MaxLength(50, ErrorMessage = "The last name cannot be more than 50 characters.")]
        public string? User_LastName { get; set; }

        [AllowNull]
        [EmailAddress(ErrorMessage = "The email does not have a valid format.")]
        public string? User_Email { get; set; }

        [AllowNull]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "The password cannot be more than 20 characters.")]
        public string? User_Password { get; set; }
        public UserInputUpdate() { }
    }

    public class UserOutput: IUser 
    {
        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string User_Email { get; set; }
        public UserOutput() { }
    }

    public class UserOutputCreate : IUserWithCreationDate
    {
        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string User_Email { get; set; }
        public DateTime User_CreationDate {  get; set; } 
        public UserOutputCreate() { }
    }
}
