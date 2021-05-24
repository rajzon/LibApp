using AutoMapper;
using Book.API.Domain;
using EventBus.Messages.Commands;

namespace Book.API.Mappings
{
    public class EventsMappingsProfile : Profile
    {
        public EventsMappingsProfile()
        {
            CreateMap<Language, LanguageDto>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Author, AuthorDto>();
            CreateMap<Image, ImageDto>();
            CreateMap<AuthorName, AuthorNameDto>();

            CreateMap<Domain.Book, CreateBook>()
                .ForMember(dest => dest.Ean13, opt => opt.MapFrom(src => src.Ean13.Code))
                .ForMember(dest => dest.Isbn10, opt => opt.MapFrom(src => src.Isbn10.Code))
                .ForMember(dest => dest.Isbn13, opt => opt.MapFrom(src => src.Isbn13.Code));
        }
    }
}