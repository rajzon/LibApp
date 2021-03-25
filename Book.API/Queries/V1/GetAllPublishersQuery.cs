using System.Collections.Generic;
using Book.API.Controllers.V1;
using Book.API.Queries.V1.Dtos;
using MediatR;

namespace Book.API.Queries.V1
{
    public class GetAllPublishersQuery : IRequest<IEnumerable<PublisherDto>>
    {
    }
}