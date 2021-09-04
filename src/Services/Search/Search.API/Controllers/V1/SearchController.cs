using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Search.API.Application.Services;
using Search.API.Commands;
using Search.API.Commands.V1;
using Search.API.Contracts.Responses;
using Search.API.Helpers.EqualityComparers;

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


        [HttpGet("book/delivery/{searchTerm}")]
        [ProducesResponseType(typeof(BookDeliveryResponse), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> BookEanSearch(string searchTerm)
        {
            var result =
                await _bookRepository.SearchByEanAsync(new SearchBookByEanCommand() {SearchTerm = searchTerm});

            if (! result.Documents.Any())
                return NotFound();

            var response = _mapper.Map<BookDeliveryResponse>(result.Documents.FirstOrDefault());

            return Ok(response);
        }

        [HttpGet("suggest/book/management/{searchSuggestValue}")]
        [ProducesResponseType(typeof(List<SuggestBookManagementResult>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> BookManagementSuggest(string searchSuggestValue)
        {
            var result = await _bookRepository.SuggestAsync(new SuggestBookCommand
                {SearchSuggestValue = searchSuggestValue});

            if (!result.IsValid)
                return NotFound();
            
            var response = (from suggests in result.Suggest.Values 
                from suggest in suggests 
                from option in suggest.Options 
                select new SuggestBookManagementResult(option)).ToList();
            
            return Ok(response);
        }

        [HttpGet("customer/{searchTerm}")]
        [ProducesResponseType(typeof(IReadOnlyCollection<CustomerResponse>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> CustomerSearch(string searchTerm)
        {
            var result = await _bookRepository.SearchCustomersByEmail(new SearchCustomerCommand() {SearchTerm = searchTerm});
            
            if (!result.Documents.Any())
                return NotFound();

            var response = _mapper.Map<CustomerResponse>(result.Documents.FirstOrDefault());

            return Ok(response);
        }

        [HttpGet("suggest/customer/{suggestValue}")]
        [ProducesResponseType(typeof(List<SuggestCustomerResult>), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> CustomersSuggest(string suggestValue)
        {
            var result = await _bookRepository.SuggestCustomerAsync(new SuggestCustomerCommand()
                {SuggestValue = suggestValue});
            
            if (!result.IsValid)
                return NotFound();
            
            var response = (from suggests in result.Suggest.Values 
                from suggest in suggests 
                from option in suggest.Options 
                select new SuggestCustomerResult(option)).ToList();

            return Ok(response.Distinct(new SuggestCustomerResultEqualityComparer()));
        }
    }
    
}