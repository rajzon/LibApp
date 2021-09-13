using AutoMapper;
using Lend.API.Controllers.V1;
using Lend.API.Domain;

namespace Lend.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AddressBasket, AddressBasketDto>()
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode.Code));

            CreateMap<AddressCorrespondenceBasket, AddressCorrespondenceBasketDto>()
                .ForMember(dest => dest.PostCode, opt => opt.MapFrom(src => src.PostCode.Code));

            CreateMap<CustomerBasket, CustomerBasketDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.EmailAddress))
                .ForMember(dest => dest.PersonIdCard, opt => opt.MapFrom(src => src.PersonIdCard.Value));
            CreateMap<Basket, BasketResponseDto>();


            CreateMap<CategoryBasket, CategoryBasketDto>();
            CreateMap<AuthorBasket, AuthorBasketDto>();
            CreateMap<PublisherBasket, PublisherBasketDto>();
            CreateMap<ImageBasket, ImageBasketDto>();
            CreateMap<StockWithBooksBasket, StockWithBooksBasketDto>();
        }
    }
}