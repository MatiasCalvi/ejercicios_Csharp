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
            try
            { 
                UserOutput userOutput = await _userServices.VerifyUserAsync(user.User_Email, user.User_Password);
                if (userOutput == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                var token = _validateMethodes.GenerateAccessToken(userOutput);
                var refreshToken = await _validateMethodes.GenerateAndStoreRefreshTokenAsync(userOutput.User_ID, userOutput.User_Role);

                return Ok(new { Token = token, RefreshToken = refreshToken });
            
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during login", Detail = ex.Message });
            }
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            try
            {
                int userId = _validateMethodes.GetUserIdFromToken();

                await _validateMethodes.DeleteRefreshTokenAsync(userId);

                _validateMethodes.DeleteCookie("RefreshToken");

                return Ok("Logout successful");
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during logout", Detail = ex.Message });
            }
        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            try
            {
      
                var userRoleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

                if (string.IsNullOrEmpty(userRoleClaim))
                {
                    return Unauthorized("User not authenticated or missing role claim.");
                }

                var userIdClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid)?.Value;

                if (!int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized("Invalid UserId claim.");
                }

                var refreshToken = await _validateMethodes.GetRefreshTokenAsync(userId);

                if (string.IsNullOrEmpty(refreshToken))
                {
                    return Unauthorized("Refresh token not found in the database.");
                }

                UserOutput userOutput = await _userServices.GetInformationFromUserAsync(userId);

                var token = _validateMethodes.GenerateAccessToken(userOutput);

                var cookie = Request.Cookies["RefreshToken"];
                if (cookie == null)
                {
                    return Unauthorized("Refresh token is missing or invalid.");
                }

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