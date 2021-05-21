using System.Threading;
using System.Threading.Tasks;
using Book.API.Domain;
using Book.API.Queries.V1;
using Book.API.Queries.V1.Dtos;
using MediatR;

namespace Book.API.Handlers
{
    public class FindLanguageByIdQueryHandler : IRequestHandler<FindLanguageByIdQuery, LanguageDto>
    {
        private readonly ILanguageRepository _languageRepository;
        
        public FindLanguageByIdQueryHandler(ILanguageRepository languageRepository)
        {
            _languageRepository = languageRepository;
        }

        public async Task<LanguageDto> Handle(FindLanguageByIdQuery request, CancellationToken cancellationToken)
        {
            var language = await _languageRepository.FindByIdAsync(request.Id);

            return language is not null
                ? new LanguageDto()
                {
                    Id = language.Id,
                    Name = language.Name
                } : null;
        }
    }
}