using apiWeb_MVC.Services;
using Datos.Exceptions;
using Datos.Interfaces;
using Datos.Schemas;
using Datos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace apiWeb_MVC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "admin")]
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
        public async Task<List<Author>> GetAll()
        {
            try
            {
                List<Author> authors = await _authorServices.GetAllAuthorsAsync();

                return authors;
            }
            catch (Exception ex)
            {
                throw new DatabaseQueryException("Failed to get all authors.", ex);
            }
        }

        [HttpGet("GetAuthor")]
        public async Task<IActionResult> GetAuthor(string authorName)
        {
            try 
            {
                authorName = authorName.ToUpper();
                Author author = await _authorServices.GetAuthorByNameAsync(authorName);

                return Ok(author);
            
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during Search", Detail = ex.Message }); 
            }
        }

        [HttpGet("GetAuthorById")]
        public async Task<IActionResult> GetAuthorById([FromQuery] int id)
        {
            try
            {
                Author author = await _authorServices.GetInformationFromAuthorAsync(id);

                if (author == null)
                {
                    return NotFound("Author not found");
                }

                return Ok(author);

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during Search", Detail = ex.Message }); 
            }
        }

        [HttpPost("CreateAuthor")]
        public async Task<IActionResult> CreateAuthor(string pAuthor)
        {
            try 
            {
                pAuthor = pAuthor.ToUpper();

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Author authorExist = await _authorServices.GetAuthorByNameAsync(pAuthor);

                if (authorExist != null)
                {
                    return BadRequest("The author already exists.");
                }

                AuthorCreate author = await _authorServices.CreateNewAuthorAsync(pAuthor);

                if (author != null)
                {
                    return CreatedAtAction(nameof(CreateAuthor), new { id = author.Author_Id }, author);
                }
                else
                {
                    return BadRequest("There was a problem creating the author.");
                }
             
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during author creation", Detail = ex.Message });
            }
        }

        [HttpPatch("UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor([FromQuery] int id, [FromBody] AutorInputUP authorInput)
        {
            try
            {
                authorInput.Author_Name = authorInput.Author_Name.ToUpper();

                Author user = await _authorServices.GetInformationFromAuthorAsync(id);
                if (user == null) return NotFound("Author not found.");

                AuthorUpdateOut updatedauthor = await _authorServices.UpdateAuthorAsync(id, authorInput);

                if (updatedauthor != null) return Ok(updatedauthor);

                else return BadRequest("There was a problem updating the author.");

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during author update", Detail = ex.Message });
            }
        }

        [HttpPatch("DisableAuthor")]
        public async Task<IActionResult> DisableAuthor([FromQuery] int id)
        {
            try
            {
                bool result = await _authorServices.DisableAuthorAsync(id);
                
                if (result)
                {
                    return NoContent();
                }
                else
                {
                    return NotFound("Author not found or already disabled.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during author disable", Detail = ex.Message });
            }

        }

        [HttpDelete("DeleteAuthor")]
        public async Task<IActionResult> DeleteAuthor([FromQuery] int id)
        {
            try
            {
                Author author = await _authorServices.GetInformationFromAuthorAsync(id);

                if (author == null)
                {
                    return NotFound("Author not found.");
                }
                else
                {
                    await _authorServices.DeletedAuthorAsync(id);
                    return NoContent();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = "Error during author deletion", Detail = ex.Message });
            }
        }
    }
}
