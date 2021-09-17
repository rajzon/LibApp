using System;
using System.Collections.Generic;
using EventBus.Messages.Events;

namespace EventBus.Messages.Commands
{
    public class CreateSeededCustomers : IntegrationEvent
    {
        public IEnumerable<CustomerDto> Customers { get; set; }
    }

    public class CustomerDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public EmailDto Email { get; set; }
        public IdCardDto PersonIdCard { get; set; }
        public IdentityTypeDto IdentityType { get; set; }
        public long Phone { get; set; }
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
        public PostCodeDto PostCode { get; set; }
        public string Post { get; set; }
        public string Country { get; set; }
    }

    public class PostCodeDto
    {
        public string Code { get; set; }
    }


    public class AddressCorrespondenceDto
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public string City { get; set; }
        public PostCodeDto PostCode { get; set; }
        public string Post { get; set; }
        public string Country { get; set; }
    }

    public class IdCardDto 
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    
    public class EmailDto 
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
    
    public enum IdentityTypeDto
    {
        PersonIdCard,
        Passport
    }
}