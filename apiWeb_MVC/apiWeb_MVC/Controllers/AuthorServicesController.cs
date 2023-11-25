using Datos.Interfaces;
using Datos.Schemas;
using Microsoft.AspNetCore.Mvc;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthorServicesController : ControllerBase
    {
        private readonly ILogger<AuthorServicesController> _logger;
        private IAuthorServices _authorServices;
        public AuthorServicesController(ILogger<AuthorServicesController> logger, IAuthorServices authorServices)
        {
            _logger = logger;
            _authorServices = authorServices;
        }

        [HttpGet("GetAll")]
        public List<Author> GetAll()
        {
            List<Author> authors = _authorServices.GetAllUsers();

            return authors;
        }

        [HttpGet("GetAuthor")]
        public Author GetAuthor(string authorName)
        {   
            Author author = _authorServices.GetAuthorByName(authorName);

            return author;
        }

        [HttpGet("GetAuthorById")]

        public IActionResult GetUser([FromQuery] int id)
        {
            Author user = _authorServices.GetInformationFromAuthor(id);
            if (user == null)
            {
                return NotFound("author not found");
            }
            return Ok(user);
        }

        [HttpPost("CreateAuthor")]
        public IActionResult CreateBookWithAuthorName( string pAuthor)
        {

            pAuthor.ToUpper();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Author authorExist = _authorServices.GetAuthorByName(pAuthor);

            if (authorExist != null)
            {
                return BadRequest("The author already exists.");
            }

            Author author = _authorServices.CreateNewAuthor(pAuthor);

            if (author != null)
            {
                return CreatedAtAction(nameof(GetAuthor), new { id = author.Author_Id }, author);
            }
            else
            {
                return BadRequest("There was a problem creating the author.");
            }
        }

    }
}
