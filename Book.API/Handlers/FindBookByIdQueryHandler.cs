using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Domain;
using Book.API.Queries.V1;
using Book.API.Queries.V1.Dtos;
using MediatR;

namespace Book.API.Handlers
{
    public class FindBookByIdQueryHandler : IRequestHandler<FindBookByIdQuery, BookDto>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public FindBookByIdQueryHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }


        public async Task<BookDto> Handle(FindBookByIdQuery request, CancellationToken cancellationToken)
        {
            var book = await _bookRepository.FindByIdAsync(request.Id);

            return book is null ? null : _mapper.Map<BookDto>(book);
        }
    }
}