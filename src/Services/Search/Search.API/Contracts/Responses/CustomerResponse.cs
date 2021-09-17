using System;
using Search.API.Domain;

namespace Search.API.Contracts.Responses
{
    public class CustomerResponse
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PersonIdCard { get; set; }
        public IdentityType IdentityType { get; set; }
        public string Nationality { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public AddressDto Address { get; set; }
        public AddressCorrespondenceDto CorrespondenceAddress { get; set; }
        
    }

    public class AddressDto
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Post { get; set; }
        public string Country { get; set; }
    }

    public class AddressCorrespondenceDto
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public string City { get; set; }
        public string PostCode { get; set; }
        public string Post { get; set; }
        public string Country { get; set; }
    }
}