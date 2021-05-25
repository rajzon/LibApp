using AutoMapper;
using EventBus.Messages.Commands;
using Search.API.Contracts.Responses;
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
            CreateMap<AuthorNameDto, AuthorName>();
            CreateMap<AuthorDto, Author>();
            CreateMap<ImageDto, Image>();

            CreateMap<CreateBook, Book>();
            ///
            //////////
            CreateMap<Category, CategoryResponseDto>();
            CreateMap<Language, LanguageResponseDto>();
            CreateMap<Publisher, PublisherResponseDto>();
            CreateMap<AuthorName, AuthorNameResponseDto>();
            CreateMap<Author, AuthorResponseDto>();
            CreateMap<Image, ImageResponseDto>();
            
            CreateMap<Book, BookManagementResponseDto>();
        }
    }
}