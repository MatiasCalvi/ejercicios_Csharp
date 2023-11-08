using Datos.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Datos.Schemas
{
    public class UserInput : IUser 
    {
        public int User_ID { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        [MaxLength(50, ErrorMessage = "The name cannot be more than 50 characters.")]        
        public string User_Name { get; set; }

        [Required(ErrorMessage = "The last name is mandatory.")]
        [MaxLength(50, ErrorMessage = "The last name cannot be more than 50 characters.")]
        public string User_LastName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "The email does not have a valid format.")]
        public string User_Email { get; set; }

        [Required(ErrorMessage = "The password is required.")]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "The password cannot be more than 20 characters.")]
        public string User_Password { get; set; }

        public UserInput() { }

    }
    public class UserOutput : IUser
    {
        public int User_ID { get; set; }
        public string User_Name { get; set; }
        public string User_LastName { get; set; }
        public string User_Email { get; set; }   
        public UserOutput() { }
    }
    public class UserUpdate
    {
        [AllowNull]
        [MaxLength(50, ErrorMessage = "The name cannot be more than 50 characters.")]
        public string? User_Name { get; set; }

        [AllowNull]
        [MaxLength(50, ErrorMessage = "The last name cannot be more than 50 characters.")]
        public string? User_LastName { get; set; }
        
        [AllowNull]
        [EmailAddress(ErrorMessage = "The email does not have a valid format.")]
        public string? User_Email { get; set; }
        
        [AllowNull]
        [MinLength(6, ErrorMessage = "The password must be at least 6 characters.")]
        [MaxLength(20, ErrorMessage = "The password cannot be more than 20 characters.")]
        public string? User_Password { get; set; }

        public UserUpdate() { }
    }
}
