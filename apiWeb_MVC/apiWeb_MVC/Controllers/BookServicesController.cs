using Datos.Interfaces;
using Datos.Schemas;
using Datos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin,user")]
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
        [Authorize(Roles = "admin")]
        public async Task<List<BookOutput>> GetAll()
        {
            try
            {
                List<BookOutput> books = await _bookServices.GetAllBooksAsync();
                return books;
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to get all books.", ex);
            }
        }

        [HttpGet("GetBook")]
        public async Task<IActionResult> GetBook([FromQuery] int id)
        {
            try
            {
                BookOutput book = await _bookServices.GetBookByIdAsync(id);

                if (book == null)
                {
                    return NotFound("Book not found");
                }

                return Ok(book);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error getting book: {ex.Message}");
            }
        }

        [HttpPost("CreateBook")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateBookWithAuthorName([FromBody] BookWithAuthorID bookInput)
        {
            bookInput.Book_Name.ToUpper();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            BookOutput bookExist = await _bookServices.GetBookByNameAsync(bookInput.Book_Name);

            if (bookExist != null)
            {
                return BadRequest("The name of the book already exists");
            }

            try
            {
                BookOutput bookOutput = await _bookServices.CreateNewBookWithAuthorNameAsync(bookInput);

                if (bookOutput != null)
                {
                    return CreatedAtAction(nameof(GetBook), new { id = bookOutput.Book_ID }, bookOutput);
                }
                else
                {
                    return BadRequest("There was a problem creating the book.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error creating book: {ex.Message}");
            }
        }

        [HttpPatch("UpdateBook")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateBook([FromQuery] int id, [FromBody] BookInputUpdateAidString bookInput)
        {
            try
            {
                BookOutput book = await _bookServices.GetBookByIdAsync(id);

                if (book == null)
                {
                    return NotFound("Book not found.");
                }

                BookOutput bookExist = await _bookServices.GetBookByNameAsync(bookInput.Book_Name.ToUpper());

                if (bookExist != null)
                {
                    return BadRequest("The name of the book already exists in the database.");
                }

                if (bookInput.Book_AuthorID != null)
                {
                    Author authorExis = await _authorServices.GetAuthorByNameAsync(bookInput.Book_AuthorID.ToUpper());

                    if (authorExis == null)
                    {
                        return NotFound("The author does not exist in the database, you must create it first to make this request");
                    }
                }

                bookInput.Book_AuthorID = bookInput.Book_AuthorID.ToUpper();
                bookInput.Book_Name = bookInput.Book_Name.ToUpper();

                BookOutput updatedBook = await _bookServices.UpdateBookAsync(id, bookInput);

                if (updatedBook != null)
                {
                    return Ok(updatedBook);
                }
                else
                {
                    return BadRequest("There was a problem updating the book.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating book: {ex.Message}");
            }
        }

        [HttpPatch("DisableBook")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DisableBook([FromQuery] int id)
        {
            bool result = await _bookServices.DisableBookAsync(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound("Book not found or already disabled.");
            }
        }

        [HttpDelete("DeleteBook")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteBook([FromQuery] int id)
        {
            try
            {
                BookOutput book = await _bookServices.GetBookByIdAsync(id);

                if (book == null)
                {
                    return NotFound("book not found.");
                }
                else
                {
                    await _bookServices.DeletedBookAsync(id);
                    return NoContent();
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error during book deletion: {ex.Message}");
            }
        }

        [HttpGet("print")]
        public async Task<IActionResult> PrintBooksAndAuthors()
        {
            var book = await _bookServices.GetBooksAndAuthorsAsync();
            return Ok(book);
        }
    }
}
