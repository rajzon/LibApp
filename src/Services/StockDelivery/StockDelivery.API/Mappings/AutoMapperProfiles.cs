using System.Linq;
using AutoMapper;
using StockDelivery.API.Commands.V1.Dtos;
using StockDelivery.API.Domain;
using StockDelivery.API.Queries.V1.Dtos;

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


            CreateMap<ActiveDelivery, ActiveDeliveryDto>()
                .ForMember(dest => dest.BooksCount,
                    opt => opt.MapFrom(src => src.Items.Count))
                // .ForMember(dest => dest.ItemsCount,
                //     opt => opt.MapFrom(src => src.Items.Aggregate(0, (total, next) => total + next.ItemsCount)))
                .ForMember(dest => dest.ItemsCount,
                    opt => opt.MapFrom(src => src.Items.Sum(i => i.ItemsCount)));
        }
    }
}