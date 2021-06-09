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
            CreateMap<Author, AuthorResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name.FullName));
                
            CreateMap<Image, ImageResponseDto>();

            CreateMap<Book, BookManagementResponseDto>();
        }
    }
}