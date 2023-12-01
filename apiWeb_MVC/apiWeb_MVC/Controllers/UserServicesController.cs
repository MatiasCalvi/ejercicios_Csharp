using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;
using Datos.Interfaces;
using Configuracion;
using Microsoft.Extensions.Options;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserServicesController : ControllerBase
    {
        private IUserServices _userServices;

        private readonly ILogger<UserServicesController> _logger;
        public UserServicesController(ILogger<UserServicesController> logger, IUserServices userServices, IOptions<JwtConfiguration>jwtConfiguration)
        {
            _logger = logger;
            _userServices = userServices;
        }

        [HttpGet("GetAll")]

        public async Task<List<UserOutput>> GetAll()
        {
            List<UserOutput> user = await _userServices.GetAllUsersAsync();

            return user;
        }

        [HttpGet("GetUser")]

        public async Task <IActionResult> GetUser([FromQuery] int id)
        {
            UserOutput user = await _userServices.GetInformationFromUserAsync(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers([FromQuery] string ids)
        {
            List<int> userIds = ids.Split(',').Select(int.Parse).ToList();
            List<UserOutput> users = await _userServices.GetUsersByIdsAsync(userIds);

            if (users.Count == 0)
            {
                return NotFound("No users found for the IDs provided.");
            }

            return Ok(users);
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserInput userInput)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            UserInputUpdate user = await _userServices.GetUserByEmailAsync(userInput.User_Email);
            if (user != null)
            {
                return BadRequest("Email already in use.");
            }

            UserOutputCreate userOutput = await _userServices.CreateNewUserAsync(userInput);

            if (userOutput != null)
            {
                return CreatedAtAction(nameof(GetUser), new { id = userOutput.User_ID }, userOutput);
            }
            else
            {
                return BadRequest("There was a problem creating the user.");
            }
        }

        [HttpPatch("DisableUser")]
        public async Task<IActionResult> DisableUser([FromQuery] int id)
        {
            bool result = await _userServices.DisableUserAsync(id);

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
        public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromBody] UserInputUpdate userInput)
        {
            UserOutput user = await _userServices.GetInformationFromUserAsync(id);
            if (user == null) return NotFound("User not found.");

            UserOutput updatedUser = await _userServices.UpdateUserAsync(id, userInput);

            if (updatedUser != null) return Ok(updatedUser);

            else return BadRequest("There was a problem updating the user.");
        }

        [HttpDelete("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            UserOutput user = await _userServices.GetInformationFromUserAsync(id);

            if (user == null)
            {
                return NotFound("User not found.");
            }
            else
            {
                await _userServices.DeletedUserAsync(id);
                return NoContent();
            }
        }
    }
}
