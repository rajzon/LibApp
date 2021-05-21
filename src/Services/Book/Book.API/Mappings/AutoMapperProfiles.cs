using AutoMapper;
using Book.API.Commands.V1;
using Book.API.Commands.V1.Dtos;
using Book.API.Controllers.V1;
using Book.API.Domain;
using Book.API.Queries.V1.Dtos;
using EventBus.Messages.Events;

namespace Book.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Language, LanguageDto>();
            CreateMap<Publisher, PublisherDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Image, CommandPhotoDto>();
            
            CreateMap<Domain.Book, CommandBookDto>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title));

            CreateMap<Domain.Book, BookDto>()
                .ForMember(dest => dest.Ean13, opt => opt.MapFrom(src => src.Ean13.Code))
                .ForMember(dest => dest.Isbn10, opt => opt.MapFrom(src => src.Isbn10.Code))
                .ForMember(dest => dest.Isbn13, opt => opt.MapFrom(src => src.Isbn13.Code));


            CreateMap<Author, AuthorDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name.FullName));
            

            //TODO only for test purposes
            CreateMap<Domain.Book, CommandBookDto>()
                .ForMember(dest => dest.Ean13, opt => opt.MapFrom(src => src.Ean13.Code))
                .ForMember(dest => dest.Isbn10, opt => opt.MapFrom(src => src.Isbn10.Code))
                .ForMember(dest => dest.Isbn13, opt => opt.MapFrom(src => src.Isbn13.Code));

            CreateMap<Domain.Book, CreateBookEvent>()
                .ForMember(dest => dest.Ean13, opt => opt.MapFrom(src => src.Ean13.Code))
                .ForMember(dest => dest.Isbn10, opt => opt.MapFrom(src => src.Isbn10.Code))
                .ForMember(dest => dest.Isbn13, opt => opt.MapFrom(src => src.Isbn13.Code));
        }
    }
}