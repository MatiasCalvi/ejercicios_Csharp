using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;
using Datos.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Configuracion;
using Microsoft.Extensions.Options;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserServicesController : ControllerBase
    {
        private IUserServices _userServices;
        private JwtConfiguration _jwtConfiguration;

        private readonly ILogger<UserServicesController> _logger;
        public UserServicesController(ILogger<UserServicesController> logger, IUserServices userServices, IOptions<JwtConfiguration>jwtConfiguration)
        {
            _logger = logger;
            _userServices = userServices;
            _jwtConfiguration = jwtConfiguration.Value;
        }

        [HttpGet("GetAll")]

        public List<UserOutput> GetAll()
        {
            List<UserOutput> user = _userServices.GetAllUsers();

            return user;
        }

        [HttpGet("GetUser")]

        public IActionResult GetUser([FromQuery] int id)
        {
            UserOutput user = _userServices.GetInformationFromUser(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpGet("GetUsers")]
        public IActionResult GetUsers([FromQuery]string ids)
        {
            List<int> userIds = ids.Split(',').Select(int.Parse).ToList();
            List<UserOutput> users = _userServices.GetUsersByIds(userIds);

            if (users.Count == 0)
            {
                return NotFound("No users found for the IDs provided.");
            }

            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserInput userInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserInputUpdate user = _userServices.GetUserByEmail(userInput.User_Email);
            if (user != null)
            {
                return BadRequest("Email already in use.");
            }
            
            UserOutputCreate userOutput = _userServices.CreateNewUser(userInput);

            if (userOutput != null)
            {
                return CreatedAtAction(nameof(GetUser), new { id = userOutput.User_ID }, userOutput);
            }
            else
            {
                return BadRequest("There was a problem creating the user.");
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin user)
        {
            UserOutput userOutput = _userServices.VerifyUser(user.User_Email, user.User_Password);
            if (userOutput == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfiguration.Secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid,userOutput.User_ID.ToString()),
                new Claim(ClaimTypes.Name,userOutput.User_Name),
                new Claim(ClaimTypes.Email,userOutput.User_Email)
            };

            var Sectoken = new JwtSecurityToken(_jwtConfiguration.Issuer,
                _jwtConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

            return Ok(token);
        }

        [HttpGet("TestToken")]
        [Authorize]
        public string Prueba()
        {
            return "Success";
        }

        [HttpPost("PasswordVerify")]
        public IActionResult PasswordVerify([FromQuery] int id, [FromBody] UserPassword password)
        {
            try
            {
                UserInputUpdate usuarioBD = _userServices.GetInformationFromUserU(id);

                if (usuarioBD == null)
                {
                    return NotFound("User not found.");
                }

                string passwordInput = password.User_Password;
                bool correctPassword = _userServices.VerifyPassword(passwordInput, usuarioBD.User_Password);

                if (correctPassword)
                {
                    UserOutput user = _userServices.GetInformationFromUser(id);
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

        [HttpPatch("DisableUser")]
        public IActionResult DisableUser([FromQuery] int id)
        {
            bool result = _userServices.DisableUser(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound("User not found or already disabled.");
            }
        }

        [HttpPatch("UpdateUser")]
        public IActionResult UpdateUser([FromQuery] int id, [FromBody] UserInputUpdate userInput)
        {   
            UserOutput user = _userServices.GetInformationFromUser(id);
            if (user == null) return NotFound("User not found.");

            UserOutput updatedUser = _userServices.UpdateUser(id, userInput);

            if (updatedUser != null) return Ok(updatedUser);

            else return BadRequest("There was a problem updating the user.");
        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser([FromQuery] int id)
        {
            UserOutput user = _userServices.GetInformationFromUser(id); 
            if (user == null) 
            {
                return NotFound("User not found."); 
            }
            else 
            {
                _userServices.DeletedUser(id); 
                return NoContent();
            }
        }
    }
}
