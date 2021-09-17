using AutoMapper;
using EventBus.Messages.Commands;
using User.Domain;
using Customer = User.Domain.Customer;

namespace User.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Email, EmailDto>();
            CreateMap<IdCard, IdCardDto>();
            CreateMap<PostCode, PostCodeDto>();
            CreateMap<Address, AddressDto>();
            CreateMap<AddressCorrespondence, AddressCorrespondenceDto>();

            CreateMap<Customer, CustomerDto>();
        }
    }
}