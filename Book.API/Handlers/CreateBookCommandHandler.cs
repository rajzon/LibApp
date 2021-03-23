using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Book.API.Commands;
using Book.API.Data;
using Book.API.Domain;
using Book.API.Repositories;
using Book.API.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Book.API.Handlers
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookResponse>
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;


        public CreateBookCommandHandler(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        
        public async Task<BookResponse> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            var book = new Domain.Book(request.Title);

            var result = _bookRepository.Add(book);
            return _mapper.Map<BookResponse>(result);
        }
    }
}