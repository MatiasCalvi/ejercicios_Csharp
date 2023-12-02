using Datos.Interfaces;
using Datos.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin, user")]

    public class RentedServicesController : ControllerBase
    {
        private IValidateMethodes _validateMethodes;
        private IBookServices _bookServices;
        private IUserServices _userServices;
        private IRentedServices _rentedServices;

        private readonly ILogger<RentedServicesController> _logger;

        public RentedServicesController(ILogger<RentedServicesController> logger, IBookServices bookServices, IUserServices userServices, IValidateMethodes validateMethodes, IRentedServices rentedServices)
        {
            _logger = logger;
            _bookServices = bookServices;
            _userServices = userServices;
            _validateMethodes = validateMethodes;
            _rentedServices = rentedServices;
        }

        [HttpGet("GetListRented")]
        [Authorize(Roles = "admin")]
        public async Task<List<RentedBookOut>> GetListRented()
        {
            return await _rentedServices.GetAllRentedAsync();
        }

        [HttpGet("GetRent")]
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> GetRent([FromQuery] int id)
        {
            RentedBookOut rent = await _rentedServices.GetRentByIDAsync(id);
            if (rent == null)
            {
                return NotFound("Rent not found");
            }
            return Ok(rent);
        }

        [HttpPost("CreateRent")]
        [Authorize(Roles = "user")]
        public async Task<IActionResult> CreateRent([FromQuery] int bookId, [FromBody] string email)
        {
            try
            {
                int userIdToken = _validateMethodes.GetUserIdFromToken();
                UserInputUpdate user = await _userServices.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return NotFound("User not found in the database");
                }

                if (userIdToken != user.User_ID)
                {
                    return Forbid();
                }

                BookOutput book = await _bookServices.GetBookByIdAsync(bookId);

                if (book == null)
                {
                    return NotFound("Book not found in the database");
                }

                RentedBookOut newRental = await _rentedServices.CreateNewRentAsync(book, userIdToken);

                if (newRental == null)
                {
                    return BadRequest("Error creating rent");
                }

                return CreatedAtAction(nameof(GetRent), new { id = newRental.RB_Id }, newRental);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
