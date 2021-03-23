using AutoMapper;
using Book.API.Responses;

namespace Book.API.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Domain.Book, BookResponse>();
        }
    }
}