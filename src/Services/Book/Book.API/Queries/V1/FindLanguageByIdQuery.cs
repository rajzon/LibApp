using Book.API.Queries.V1.Dtos;
using MediatR;

namespace Book.API.Queries.V1
{
    public class FindLanguageByIdQuery : IRequest<LanguageDto>
    {
        public int Id { get; set; }
    }
}