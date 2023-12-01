using Datos.Interfaces;
using Datos.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
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

        public List<RentedBook> GetListRented()
        {
            List<RentedBook> list = _rentedServices.GetAllRented();
            return list;
        }

        [HttpGet("GetRent")]
        public IActionResult GetRent([FromQuery] int id)
        {
            RentedBookOut rent = _rentedServices.GetRentByID(id);
            if (rent == null)
            {
                return NotFound("Rent not found");
            }
            return Ok(rent);
        }

        [HttpPost("CreateRent")]
        [Authorize(Roles = "user")]
        public IActionResult CreateRent([FromQuery] int bookId, [FromBody] string email)
        {
            try
            {
                int userIdToken = _validateMethodes.GetUserIdFromToken();
                UserInputUpdate userIdReq = _userServices.GetUserByEmail(email);

                BookOutput bookReq = _bookServices.GetInformationFromBook(bookId);

                if (userIdReq == null)
                {
                    return NotFound("User not found in the database");
                }

                if (userIdToken != userIdReq.User_ID)
                {
                    return Forbid();
                }

                if (bookReq == null)
                {
                    return NotFound("Book not found in the database");
                }

                RentedBookOut newRental = _rentedServices.CreateNewRent(bookReq, userIdToken);

                if (newRental == null)
                {
                    BadRequest("Error creating rent");
                }
                return CreatedAtAction(nameof(GetRent), new { id = newRental.RB_Id}, newRental);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}
