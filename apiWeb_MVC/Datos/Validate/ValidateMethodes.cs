using Microsoft.AspNetCore.Http;
using Datos.Interfaces;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

namespace Datos.Validate
{
    public class ValidateMethodes : IValidateMethodes
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ValidateMethodes(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserIdFromToken()
        {
            var userIdClaim = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Sid);
            int userId;
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out userId))
            {
                return userId;
            }
            throw new ApplicationException("Could not extract User_Id from token.");
        }

        public string HashPassword(string pPassword)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pPassword.Normalize(NormalizationForm.FormKD));
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return BCrypt.Net.BCrypt.HashPassword(hashedPassword, 4);
        }

        public bool VerifyPassword(string pUserInput, string pHashedPassword)
        {

            using SHA256 sha256 = SHA256.Create();
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pUserInput.Normalize(NormalizationForm.FormKD));
            byte[] hashedBytes = sha256.ComputeHash(passwordBytes);
            string hashedPassword = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            return BCrypt.Net.BCrypt.Verify(hashedPassword, pHashedPassword);
        }
    }
}
