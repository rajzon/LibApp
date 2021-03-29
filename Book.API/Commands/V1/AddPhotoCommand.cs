using Book.API.Controllers.V1;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Book.API.Commands.V1
{
    public class AddPhotoCommand : IRequest<AddPhotoCommandResult>
    {
        [FromRoute(Name = "id")]
        public int Id { get; init; }
        public bool IsMain { get; init; }
        public IFormFile File { get; init; }

    }
}