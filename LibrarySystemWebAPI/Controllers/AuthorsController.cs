using LibrarySystemWebAPI.Entities;
using LibrarySystemWebAPI.Filters;
using LibrarySystemWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ActionExecutionLoggingFilter))]
    [ServiceFilter(typeof(ActionTimingFilter))]
    [ServiceFilter(typeof(ExceptionHandlingFilter))]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _service;

        public AuthorsController(IAuthorService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int pageNumber = 1,
            int pageSize = 10,
            string? searchTerm = null)
        {
            var result = await _service.GetAllAsync(
                pageNumber, pageSize, searchTerm);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);

            return result.IsSuccess
                ? Ok(result)
                : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Author author)
        {
            var result = await _service.CreateAsync(author);

            return result.IsSuccess
                ? Ok(result)
                : BadRequest(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Author author)
        {
            if (id != author.Id)
                return BadRequest("Id mismatch.");

            var result = await _service.UpdateAsync(author);

            return result.IsSuccess
                ? Ok(result)
                : NotFound(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);

            return result.IsSuccess
                ? Ok(result)
                : NotFound(result);
        }
    }
}