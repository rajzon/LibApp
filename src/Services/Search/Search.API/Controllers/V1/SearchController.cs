using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Search.API.Application.Services;
using Search.API.Commands;
using Search.API.Domain;

namespace Search.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("v{version:apiVersion}/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public SearchController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
    
        [HttpGet("book/management")]
        public async Task<IActionResult> BookManagementSearch(string searchTerm,
            string categories, string authors,
            string languages, string publishers,
            bool visibility, DateTime modificationDateFrom, DateTime modificationDateTo)
        {
            
            var response = await _bookRepository.SearchAsync(new SearchBookCommand()
            {
                SearchTerm = searchTerm,
                Categories = categories,
                Authors = authors,
                Languages = languages,
                Publishers = publishers,
                Visibility = visibility,
                ModificationDateFrom = modificationDateFrom,
                ModificationDateTo = modificationDateTo.Equals(default) ? DateTime.MaxValue : modificationDateTo
            });

            if (! response.IsValid)
                return NotFound();

            return Ok(response.Documents);
        }
    }
}