﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Book.API.Commands.V1;
using Book.API.Contracts.Responses;
using Book.API.Queries.V1;
using Book.API.Queries.V1.Dtos;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Book.API.Controllers.V1
{
    //TODO consider define cancellation token in controller methods
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    [Route("v{version:apiVersion}/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BookController(IMediator mediator, ILogger<BookController> logger)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BookDto>), (int) HttpStatusCode.OK)]
        [ProducesResponseType( (int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllBooksQuery());

            if (!result.Any())
                return NotFound();

            return Ok(result);
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BookDto), (int) HttpStatusCode.OK)]
        [ProducesResponseType((int) HttpStatusCode.NotFound)]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new FindBookByIdQuery {Id = id});
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        
        [HttpPost("add")]
        [Authorize(Policy = "book-write")]
        [ProducesResponseType(typeof(CreateBookCommandResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> Create(CreateBookCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();
            
            //TODO only for test purposes
            Thread.Sleep(2000);
            
            return Ok(result.Book);
        }

        [HttpPost("add-manual")]
        [Authorize(Policy = "book-write")]
        [ProducesResponseType(typeof(CreateBookCommandResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> CreateManual(CreateBookManualCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();
            
            
            //TODO only for test purposes
            Thread.Sleep(2000);
            return Ok(result.Book);
        }

        [HttpPost("{id}/add-photo")]
        [Authorize(Policy = "book-edit")]
        [ProducesResponseType(typeof(AddPhotoCommandResult), (int) HttpStatusCode.OK)]
        public async Task<IActionResult> AddPhotoToBook([FromForm]AddPhotoCommand command)
        {
            var result = await _mediator.Send(command);
            
            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();


            return Ok(result.Photo);
        }

    }
}