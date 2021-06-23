using AutoMapper;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Domain;

namespace StockDelivery.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ActiveDeliveryItem, CommandActiveDeliveryItemDto>()
                .ForMember(dest => dest.BookEan,
                    opt => opt.MapFrom(src => src.BookEan.Code));
            CreateMap<ActiveDelivery, CommandActiveDeliveryDto>();
        }
    }
}