using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lend.API.Commands.V1;
using Lend.API.Contracts.Responses;
using Lend.API.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lend.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize(Roles = "employee")]
    [Route("v{version:apiVersion}/[controller]")]
    public class LendController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LendController(IMediator mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("basket/customer/{email}")]
        public async Task<IActionResult> PostCustomerForBasket(string email)
        {
            var userNameFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await _mediator.Send(new PostCustomerForBasketCommand {UserName = userNameFromToken, CustomerEmail = email});
            if (! result.Succeeded)
                return result.Errors.Any()? BadRequest(new ErrorResponse
                {
                    Errors =  result.Errors
                }): BadRequest();
            
            return Ok(result.Basket);
        }

        // [HttpPost("basket/stock/{stockId}")]
        // public async Task<IActionResult> PostStockForBasket(int stockId)
        // {
        //     var userNameFromToken = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     var result = await _mediator.Send(new PostStockForBasketCommand {UserName = userNameFromToken, StockId = stockId});
        // }
    }
    
    public class PostStockForBasketCommand : IRequest<PostBasketCommandResult>
    {
        public string UserName { get; set; }
        public int StockId { get; init; }
        //public BasketDto Basket { get; init; }
    }

    public class PostCustomerForBasketCommand : IRequest<PostBasketCommandResult>
    {
        public string UserName { get; init; }
        public string CustomerEmail { get; init; }
        //public BasketDto Basket { get; init; }
    }

    public class BasketDto
    {
        public string CustomerEmail { get; init; }
        public IEnumerable<StockWithBooksBasketDto> StockWithBooks { get; init; }
    }

    public class StockWithBooksBasketDto
    {
        public int StockId { get; set; }
        public string Title { get;  init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public DateTime PublishedDate { get; init; }
        public DateTime ReturnDate { get; init; }
        
        public IEnumerable<CategoryBasketDto> Categories { get; init; }
        public IEnumerable<AuthorBasketDto> Authors { get; init; }
        public PublisherBasketDto Publisher { get; init; }
        public ImageBasketDto Image { get; init; }
    }

    public class ImageBasketDto
    {
        public string Url { get; init; }
        public bool IsMain { get; init; }
    }

    public class PublisherBasketDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

    public class AuthorBasketDto
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName { get; init; }
    }

    public class CategoryBasketDto
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

    public class CustomerBasketDto
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Email { get; init; }
        public string PersonIdCard { get; init; }
        public IdentityType IdentityType { get; init; }
        public string Nationality { get; init; }
        public DateTime DateOfBirth { get; init; }
        
        public AddressBasketDto Address { get; init; }
        public AddressCorrespondenceBasketDto CorrespondenceAddress { get; init; }
    }

    public class AddressCorrespondenceBasketDto
    {
        public string Adres { get; init; }
        public string City { get; init; }
        public string PostCode { get; init; }
        public string Post { get; init; }
        public string Country { get; init; }
    }

    public class AddressBasketDto
    {
        public string Adres { get; init; }
        public string City { get; init; }
        public string PostCode { get; init; }
        public string Post { get; init; }
        public string Country { get; init; }
    }

    public class BasketResponseDto
    {
        public CustomerBasketDto Customer { get; init; }
        public IEnumerable<StockWithBooksBasketDto> StockWithBooks { get; init; }
        //Errors
        public List<string> BusinessErrors { get; set; }
        //Warnings
        public List<string> BusinessWarnings { get; set; }
    }
    
    public class PostBasketCommandResult : BaseCommandResult
    {
        public BasketResponseDto Basket { get; init; }

        public PostBasketCommandResult(bool succeeded, BasketResponseDto basket) : base(succeeded)
        {
            Basket = basket;
        }

        public PostBasketCommandResult(bool succeeded, IReadOnlyCollection<string> errors = default) : base(succeeded, errors)
        {
        }
    }
}