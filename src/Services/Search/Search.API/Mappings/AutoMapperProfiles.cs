using AutoMapper;
using EventBus.Messages.Commands;
using Search.API.Domain;

namespace Search.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<CategoryDto, Category>();
            CreateMap<LanguageDto, Language>();
            CreateMap<PublisherDto, Publisher>();
            CreateMap<AuthorDto, Author>();
            CreateMap<ImageDto, Image>();
            
            CreateMap<CreateBook, Book>();
        }
    }
}