using System.Linq;
using AutoMapper;
using Book.API.Commands.V1.Dtos;
using Book.API.Domain;
using Book.API.Queries.V1.Dtos;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using AuthorDto = Book.API.Queries.V1.Dtos.AuthorDto;
using CategoryDto = Book.API.Queries.V1.Dtos.CategoryDto;
using LanguageDto = Book.API.Queries.V1.Dtos.LanguageDto;
using PublisherDto = Book.API.Queries.V1.Dtos.PublisherDto;

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
            CreateMap<Image, AddImageToBook>();

            
            // FROM EVENTBUS
            CreateMap<Category, CategoryBusResponseDto>();
            CreateMap<Publisher, PublisherBusResponseDto>();
            CreateMap<Author, AuthorBusResponseDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Name.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Name.LastName))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name.FullName));
            CreateMap<Image, ImageBusResponseDto>();
            CreateMap<Domain.Book, BookInfoDto>()
                .ForMember(dest => dest.Ean13, opt => opt.MapFrom(src => src.Ean13.Code))
                .ForMember(dest => dest.Isbn10, opt => opt.MapFrom(src => src.Isbn10.Code))
                .ForMember(dest => dest.Isbn13, opt => opt.MapFrom(src => src.Isbn13.Code));


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

            
        }
    }
}