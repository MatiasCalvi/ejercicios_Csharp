using Datos.Interfaces;
using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookServicesController : ControllerBase
    {
        private readonly ILogger<BookServicesController> _logger;
        private IBookServices _bookServices;
        private IAuthorServices _authorServices;
        private IUserServices _userServices;
        public BookServicesController(ILogger<BookServicesController> logger, IBookServices bookServices, IAuthorServices authorServices, IUserServices userServices)
        {
            _logger = logger;
            _bookServices = bookServices;
            _authorServices = authorServices;
            _userServices = userServices;
        }

        [HttpGet("GetAll")]
        public List<BookOutput> GetAll()
        {
            List<BookOutput> books = _bookServices.GetAllBooks();

            return books;
        }
        
        [HttpGet("GetBook")]
        public IActionResult GetBook([FromQuery] int id)
        {
            BookOutput book = _bookServices.GetInformationFromBook(id);
            if (book == null)
            {
                return NotFound("Book not found");
            }
            return Ok(book);
        }

        [HttpPost("CreateBook")]
        public IActionResult CreateBookWithAuthorName([FromBody] BookWithAuthorID bookInput)
        {
            bookInput.Book_Name.ToUpper();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BookOutput bookExist = _bookServices.GetInformationFromBookName(bookInput.Book_Name); 
            if (bookExist != null)
            {
                return BadRequest("the name of the book already exists");
            }

            BookOutput bookOutput = _bookServices.CreateNewBookWithAuthorName(bookInput);

            if (bookOutput != null)
            {
                return CreatedAtAction(nameof(GetBook), new { id = bookOutput.Book_ID }, bookOutput);
            }
            else
            {
                return BadRequest("There was a problem creating the book.");
            }
        }

        [HttpPatch("UpdateBook")]
        public IActionResult UpdateBook([FromQuery] int id, [FromBody] BookInputUpdateAidString bookInput)
        {
            BookOutput book = _bookServices.GetInformationFromBook(id);
            if (book == null) return NotFound("Book not found.");

            BookOutput bookExist = _bookServices.GetInformationFromBookName(bookInput.Book_Name);
            if (bookExist != null)
            {
                return BadRequest("The name of the book already exists in the database.");
            }

            if(bookInput.Book_AuthorID != null)
            {
                Author authorExis = _authorServices.GetAuthorByName(bookInput.Book_AuthorID);
                if (authorExis == null) 
                    return NotFound("The author does not exist in the database, you must create it first to make this request");
            }

            BookOutput updatedBook = _bookServices.UpdateBook(id, bookInput);

            if (updatedBook != null) return Ok(updatedBook);

            else return BadRequest("There was a problem updating the user.");
        }
    }
}
