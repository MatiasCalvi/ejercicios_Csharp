using Microsoft.AspNetCore.Http;
using Datos.Interfaces;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Configuracion;
using Microsoft.Extensions.Options;


namespace Datos.Validate
{
    public class ValidateMethodes : IValidateMethodes
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDaoBDAccesUser _daoBDAccessUser; 
        private JwtConfiguration _jwtConfiguration;

        public ValidateMethodes(IHttpContextAccessor httpContextAccessor, IDaoBDAccesUser daoBDAccesUser, IOptions<JwtConfiguration> jwtConfiguration)
        {
            _httpContextAccessor = httpContextAccessor;
            _daoBDAccessUser = daoBDAccesUser;
            _jwtConfiguration = jwtConfiguration.Value;
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

        public string GenerateAccessToken(SigningCredentials credentials, List<Claim> claims)
        {
            var now = DateTime.UtcNow;
            var expiration = now.AddMinutes(1);

            var sectoken = new JwtSecurityTokenHandler().CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _jwtConfiguration.Issuer,
                Audience = _jwtConfiguration.Audience,
                Subject = new ClaimsIdentity(claims),
                IssuedAt = now,
                NotBefore = now,
                Expires = expiration,
                SigningCredentials = credentials,
            });

            return new JwtSecurityTokenHandler().WriteToken(sectoken);
        }

        private string GenerateRefreshToken(int userId, string userRole)
        {
            var refreshTokenSecret = Guid.NewGuid().ToString();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshTokenSecret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, userRole),
            };

            var now = DateTime.UtcNow;
            var expiration = now.AddHours(10);

            var refreshToken = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                notBefore: now,
                expires: expiration,
                signingCredentials: credentials
            );

            var refreshTokenHandler = new JwtSecurityTokenHandler();
            return refreshTokenHandler.WriteToken(refreshToken);
        }

        public async Task<string> GenerateAndStoreRefreshTokenAsync(int userId, string userRole)
        {
            string refreshToken = GenerateRefreshToken(userId, userRole);
            await _daoBDAccessUser.StoreRefreshTokenAsync(userId, refreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddHours(10)
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", refreshToken, cookieOptions);
            return refreshToken;
        }

        public async Task<bool> ValidateRefreshTokenAsync(int userId, string refreshToken)
        {
            try
            {
                return await _daoBDAccessUser.ValidateRefreshTokenAsync(userId, refreshToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Error validating the refresh token in the service.", ex);
            }
        }

        public void UpdateCookieExpiration(string refreshToken, string cookieValue)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(refreshToken);
            var expiration = token.ValidTo;

            var cookie = new CookieOptions();
            cookie.Expires = expiration;

            _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", cookieValue, cookie);
        }

        public async Task DeleteRefreshTokenAsync(int userId)
        {
            try
            {
                await _daoBDAccessUser.DeleteRefreshTokenAsync(userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error deleting refresh token.", ex);
            }
        }

    }
}
