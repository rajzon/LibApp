using System;

namespace EventBus.Messages.Results
{
    public class CustomerInfoResult
    {
        public CustomerBasketBusResponse Customer { get; init; }
    }
    
    public class CustomerBasketBusResponse
    {
        public int Id { get; set; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Email { get; init; }
        public string PersonIdCard { get; init; }
        public IdentityType IdentityType { get; init; }
        public string Nationality { get; init; }
        public DateTime DateOfBirth { get; init; }
        
        public AddressBasketBusResponse Address { get; init; }
        public AddressCorrespondenceBasketBusResponse CorrespondenceAddress { get; init; }
    }

    public class AddressCorrespondenceBasketBusResponse
    {
        public string Adres { get; init; }
        public string City { get; init; }
        public string PostCode { get; init; }
        public string Post { get; init; }
        public string Country { get; init; }
    }

    public class AddressBasketBusResponse
    {
        public string Adres { get; init; }
        public string City { get; init; }
        public string PostCode { get; init; }
        public string Post { get; init; }
        public string Country { get; init; }
    }
    
    public enum IdentityType
    {
        PersonIdCard,
        Passport
    }
}