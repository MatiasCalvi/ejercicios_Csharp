using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;
using Datos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Configuracion;
using Microsoft.Extensions.Options;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccessUserController : Controller
    {
        private IUserServices _userServices;
        private IValidateMethodes _validateMethodes;
        private JwtConfiguration _jwtConfiguration;
        private IHttpContextAccessor _httpContextAccessor;

        private readonly ILogger<AccessUserController> _logger;
        public AccessUserController(ILogger<AccessUserController> logger, IUserServices userServices, IOptions<JwtConfiguration> jwtConfiguration, IValidateMethodes validateMethodes, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _userServices = userServices;
            _jwtConfiguration = jwtConfiguration.Value;
            _validateMethodes = validateMethodes;
            _httpContextAccessor = httpContextAccessor;

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLogin user)
        {
            UserOutput userOutput = await _userServices.VerifyUserAsync(user.User_Email, user.User_Password);
            if (userOutput == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, userOutput.User_ID.ToString()),
                new Claim(ClaimTypes.Name, userOutput.User_Name),
                new Claim(ClaimTypes.Email, userOutput.User_Email),
                new Claim(ClaimTypes.Role, userOutput.User_Role)
            };

            var token = _validateMethodes.GenerateAccessToken(credentials, claims);
            var refreshToken = await _validateMethodes.GenerateAndStoreRefreshTokenAsync(userOutput.User_ID, userOutput.User_Role);

            return Ok(new { Token = token, RefreshToken = refreshToken });
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                int userId = _validateMethodes.GetUserIdFromToken();

                await _validateMethodes.DeleteRefreshTokenAsync(userId);

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(-1)
                };

                _httpContextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", "", cookieOptions);

                return Ok("Logout successful");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during logout", Detail = ex.Message });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (!await _validateMethodes.ValidateRefreshTokenAsync(request.UserId, request.RefreshToken))
                {
                    return Unauthorized("Invalid refresh token.");
                }

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, request.UserId.ToString()),
                    new Claim(ClaimTypes.Role, request.UserRole),
                };

                var token = _validateMethodes.GenerateAccessToken(credentials, claims);
                var refreshToken = await _validateMethodes.GenerateAndStoreRefreshTokenAsync(request.UserId, request.UserRole);

                var cookie = Request.Cookies["RefreshToken"];
                
                if(cookie == null) return Unauthorized("Refresh token is missing or invalid.");

                _validateMethodes.UpdateCookieExpiration(refreshToken, cookie);

                return Ok(new { Token = token, RefreshToken = refreshToken });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("TestToken")]
        [Authorize]
        public string Prueba()
        {
            return "Success";
        }

        [HttpPost("PasswordVerify")]
        public async Task<IActionResult> PasswordVerify([FromQuery] int id, [FromBody] UserPassword password)
        {
            try
            {
                UserInputUpdate usuarioBD = await _userServices.GetInformationFromUserUAsync(id);

                if (usuarioBD == null)
                {
                    return NotFound("User not found.");
                }

                string passwordInput = password.User_Password;

                bool correctPassword = _validateMethodes.VerifyPassword(passwordInput, usuarioBD.User_Password);

                if (correctPassword)
                {
                    UserOutput user = await _userServices.GetInformationFromUserAsync(id);
                    return Ok(user);
                }
                else
                {
                    return Unauthorized("Password does not match.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Codigo = 404, Mensaje = "Failed to verify password.", Detalle = ex.Message });
            }
        }
    }
}