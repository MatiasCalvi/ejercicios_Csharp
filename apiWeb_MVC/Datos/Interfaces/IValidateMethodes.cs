using Datos.Schemas;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Datos.Interfaces
{
    public interface IValidateMethodes
    {
        public int GetUserIdFromToken();
        Task<string> GetRefreshTokenAsync(int userId);
        public bool VerifyPassword(string pUserInput, string pHashedPassword);
        public string HashPassword(string pPassword);
        Task<string> GenerateAndStoreRefreshTokenAsync(int userId, string userRole);
        Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken);
        Task DeleteRefreshTokenAsync(int userId);
        public void UpdateCookieExpiration(string refreshToken, string cookieValue);
        public string GenerateAccessToken(UserOutput user);
        public void DeleteCookie(string nameCookie);
    }
}
