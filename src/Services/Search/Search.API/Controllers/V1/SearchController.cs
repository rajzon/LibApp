using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using Search.API.Application.Services;
using Search.API.Commands;
using Search.API.Contracts.Responses;
using Search.API.Domain;

namespace Search.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public SearchController(IBookRepository bookRepository,
            IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
    
        [HttpGet("book/management")]
        [ProducesResponseType(typeof(IReadOnlyCollection<BookManagementResponse>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> BookManagementSearch(string searchTerm,
            [FromQuery] string[] categories, [FromQuery] string[] authors,
            [FromQuery] string[] languages, [FromQuery] string[] publishers,
            [FromQuery] bool?[] visibility, string sortBy, short fromPage, short pageSize,
            DateTime modificationDateFrom, DateTime modificationDateTo)
        {
            var response = await _bookRepository.SearchAsync(new SearchBookCommand()
            {
                SearchTerm = searchTerm,
                Categories = categories,
                Authors = authors,
                Languages = languages,
                Publishers = publishers,
                Visibility = visibility,
                SortBy = sortBy,
                FromPage = fromPage,
                PageSize = pageSize,
                ModificationDateFrom = modificationDateFrom,
                ModificationDateTo = modificationDateTo.Equals(default) ? DateTime.MaxValue : modificationDateTo
            });

            if (! response.IsValid)
                return NotFound();
            

            return Ok(new BookManagementResponse(response)
            {
                Total = response.Total,
                Results = _mapper.Map<IReadOnlyCollection<BookManagementResponseDto>>(response.Documents)
            });
        }
    }
}