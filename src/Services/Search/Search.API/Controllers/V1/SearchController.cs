﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Search.API.Application.Services;
using Search.API.Commands;
using Search.API.Contracts.Responses;
using Search.API.Domain;

namespace Search.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
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
            string categories, string authors,
            string languages, string publishers,
            bool? visibility, string sortBy, short fromPage, short pageSize,
            DateTime modificationDateFrom, DateTime modificationDateTo)
        {
            //TODO add fluent validation or filter
            if (fromPage < 1 || pageSize < 1)
                return BadRequest("at least one pagination parameter is less then 1");
            
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