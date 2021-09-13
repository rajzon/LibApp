using System.Linq;
using AutoMapper;
using EventBus.Messages.Commands;
using EventBus.Messages.Results;
using Search.API.Contracts.Responses;
using Search.API.Domain;
using AddressCorrespondenceDto = Search.API.Contracts.Responses.AddressCorrespondenceDto;
using AddressDto = Search.API.Contracts.Responses.AddressDto;

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

            CreateMap<EmailDto, Email>();
            CreateMap<IdCardDto, IdCard>();
            CreateMap<PostCodeDto, PostCode>();
            CreateMap<EventBus.Messages.Commands.AddressDto, Address>();
            CreateMap<EventBus.Messages.Commands.AddressCorrespondenceDto, AddressCorrespondence>();

            CreateMap<CustomerDto, Customer>();
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
            CreateMap<Book, BookDeliveryResponse>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Images.FirstOrDefault(i => i.IsMain)));
            
            ////
            //////////
            CreateMap<Address, AddressDto>()
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode.Code));
            CreateMap<AddressCorrespondence, AddressCorrespondenceDto>()
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode.Code));

            CreateMap<Customer, CustomerResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.EmailAddress))
                .ForMember(dest => dest.PersonIdCard, opt => opt.MapFrom(src => src.PersonIdCard.Value));
            
            
            CreateMap<Address, AddressBasketBusResponse>()
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode.Code));
            CreateMap<AddressCorrespondence, AddressCorrespondenceBasketBusResponse>()
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode.Code));

            
            CreateMap<Customer, CustomerBasketBusResponse>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.EmailAddress))
                .ForMember(dest => dest.PersonIdCard, opt => opt.MapFrom(src => src.PersonIdCard.Value));

        }
    }
}