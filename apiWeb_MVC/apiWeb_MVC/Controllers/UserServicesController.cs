using Daos;
using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;
using apiWeb_MVC.Services;
using Ninject;
using Datos.Interfaces;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class UserServicesController : ControllerBase
    {
        static IKernel kernel = NinjectConfig.CreateKernel();
        IDaoBD dao => kernel.Get<IDaoBD>();
        UserServices userServices => new(dao);

        private readonly ILogger<UserServicesController> _logger;
        public UserServicesController(ILogger<UserServicesController> logger)
        {
            _logger = logger;
        }

        [HttpGet("GetAll")]

        public List<UserOutput> GetAll()
        {
            List<UserOutput> user = userServices.GetAllUsers();
            return user;
        }

        [HttpGet("GetUser")]

        public IActionResult GetUser([FromQuery] int id)
        {
            UserOutput user = userServices.GetInformationFromUser(id);
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
            List<UserOutput> users = userServices.GetUsersByIds(userIds);

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

            UserOutput user = userServices.CreateNewUser(userInput); 

            if (user != null)
            {
                return CreatedAtAction(nameof(GetUser), new { id = user.User_ID }, user); 
            }
            else
            {
                return BadRequest("There was a problem creating the user.");
            }
        }

        [HttpPatch("UpdateUser")]
        public IActionResult UpdateUser([FromQuery] int id, [FromBody] UserUpdate userInput)
        {   
            UserOutput user = userServices.GetInformationFromUser(id);
            if (user == null) return NotFound("User not found.");

            UserOutput updatedUser = userServices.UpdateUser(id, userInput);

            if (updatedUser != null) return Ok(updatedUser);

            else return BadRequest("There was a problem updating the user.");

        }

        [HttpDelete("DeleteUser")]
        public IActionResult DeleteUser([FromQuery] int id)
        {
            UserOutput user = userServices.GetInformationFromUser(id); 
            if (user == null) 
            {
                return NotFound("User not found."); 
            }
            else 
            {
                userServices.DeletedUser(id); 
                return Ok("User deleted successfully."); 
            }
        }
    }
}
