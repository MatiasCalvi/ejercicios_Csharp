using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;
using Datos.Interfaces;
using Configuracion;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin,user")]

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
        [Authorize(Roles = "admin")]

        public async Task<IActionResult> GetAll()
        {
            try
            {   
                List<UserOutput> user = await _userServices.GetAllUsersAsync();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode( 500, new { Message = "Error during search", Detail = ex.Message });
            }
        }

        [HttpGet("GetUser")]
        [Authorize(Roles = "admin,user")]

        public async Task <IActionResult> GetUser([FromQuery] int id)
        {
            try
            {
                UserOutput user = await _userServices.GetInformationFromUserAsync(id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode( 500, new { Message = "Error during search", Detail = ex.Message });
            }
        }

        [HttpGet("GetUsers")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetUsers([FromQuery] string ids)
        {
            try
            {
                List<int> userIds = ids.Split(',').Select(int.Parse).ToList();
                List<UserOutput> users = await _userServices.GetUsersByIdsAsync(userIds);

                if (users.Count == 0)
                {
                    return NotFound("No users found for the IDs provided.");
                }

                return Ok(users);

            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error during search", Detail = ex.Message });
            }
        }

        [HttpPost("CreateUser")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> CreateUser([FromBody] UserInput userInput)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode( 500, new { Message = "Error during user creation", Detail = ex.Message });
            }
        }

        [HttpPatch("DisableUser")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> DisableUser([FromQuery] int id)
        {
            try
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
            catch (Exception ex)
            {
                return StatusCode( 500, new { Message = "Error during user disable", Detail = ex.Message });
            }
        }

        [HttpPatch("UpdateUser")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> UpdateUser([FromQuery] int id, [FromBody] UserInputUpdate userInput)
        {
            try
            {
                UserOutput user = await _userServices.GetInformationFromUserAsync(id);
                if (user == null) return NotFound("User not found.");

                UserOutput updatedUser = await _userServices.UpdateUserAsync(id, userInput);

                if (updatedUser != null) return Ok(updatedUser);

                else return BadRequest("There was a problem updating the user.");

            }
            catch (Exception ex)
            {
                return StatusCode(500 , new { Message = "Error during user update", Detail = ex.Message });
            }
        }

        [HttpPatch("ForgottenPassword")]
        [Authorize(Roles = "admin,user")]

        public async Task<IActionResult> ForgottenPassword([FromQuery] int id, [FromBody] UserPasswordUpdate newPassword) 
        {
            try
            {   
                UserOutput user = await _userServices.GetInformationFromUserAsync(id);
                
                if (user == null) return NotFound("User not found.");
                
                string updatePassword = await _userServices.UserForgottenPasswordAsync(id, newPassword);

                if (updatePassword != null) return Ok(updatePassword);
                
                else return BadRequest("Error during password update");

            }
            catch(Exception ex) 
            {
                return StatusCode(500, new { Message = "Error during user update", Detail = ex.Message });
            }

        }

        [HttpDelete("DeleteUser")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> DeleteUser([FromQuery] int id)
        {
            try
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
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during user deletion", Detail = ex.Message });
            }
        }
    }
}
