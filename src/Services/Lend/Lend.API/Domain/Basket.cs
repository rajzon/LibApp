using System;
using System.Collections.Generic;
using Lend.API.Domain.Common;

namespace Lend.API.Domain
{
    public class Basket
    {
        public CustomerBasket Customer { get; set; }
        public IEnumerable<StockWithBooksBasket> StockWithBooks { get; set; }
    }

    public class StockWithBooksBasket
    {
        public int StockId { get; set; }
        public string Title { get;  init; }
        public string Ean13 { get; init; }
        public string Isbn10 { get; init; }
        public string Isbn13 { get; init; }
        public DateTime PublishedDate { get; init; }
        
        public IEnumerable<CategoryBasket> Categories { get; init; }
        public IEnumerable<AuthorBasket> Authors { get; init; }
        public PublisherBasket Publisher { get; init; }
        public ImageBasket Image { get; init; }
    }
    
    public record CategoryBasket
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }
    
    
    public record AuthorBasket
    {
        public int Id { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string FullName { get; init; }
    }
    
    
    public record ImageBasket
    {
        public string Url { get; init; }
        public bool IsMain { get; init; }
    }
    
    public record PublisherBasket
    {
        public int Id { get; init; }
        public string Name { get; init; }
    }

    public class CustomerBasket
    {
        public int Id { get; set; }
        public string Name { get; private set; }
        public string Surname { get; private set; }
        public EmailBasket Email { get; private set; }
        public IdCardBasket PersonIdCard { get; private set; }
        public IdentityType IdentityType { get; private set; }
        public string Nationality { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        
        public AddressBasket Address { get; private set; }
        public AddressCorrespondenceBasket CorrespondenceBasket { get; private set; }

        public CustomerBasket(string name, string surname, EmailBasket email,
            IdCardBasket personIdCard, IdentityType identityType, string nationality,
            DateTime dateOfBirth, AddressBasket address, AddressCorrespondenceBasket correspondenceBasket)
        {
            Name = name;
            Surname = surname;
            Email = email;
            PersonIdCard = personIdCard;
            IdentityType = identityType;
            Nationality = nationality;
            DateOfBirth = dateOfBirth;
            Address = address;
            CorrespondenceBasket = correspondenceBasket;
        }    
    }

    public class EmailBasket
    {
        public string EmailAddress { get; private set; }

        public EmailBasket(string email)
        {
            //TODO check if passed email is valid
            EmailAddress = email;
        }
        
        protected EmailBasket()
        {
            
        }
    }
    
    public class IdCardBasket
    {
        public string Value { get; private set; }
        public IdCardBasket(string value, IdentityType identityType)
        {
            //TODO consider different check based on identity type
            Value = value;
        }
        
        protected IdCardBasket()
        {
            
        }
    }
    
    public enum IdentityType
    {
        PersonIdCard,
        Passport
    }
    
    public class AddressBasket
    {
        public string Adres { get; private set; }
        public string City { get; private set; }
        public PostCodeBasket PostCode { get; private set; }
        public string Post { get; private set; }
        public string Country { get; private set; }

        public AddressBasket(string address, string city, PostCodeBasket postCode, string post, string country)
        {
            Adres = address;
            City = city;
            PostCode = postCode;
            Post = post;
            Country = country;
        }

        protected AddressBasket()
        {
            
        }
    }
    
    public class AddressCorrespondenceBasket
    {
        public string Adres { get; private set; }
        public string City { get; private set; }
        public PostCodeBasket PostCode { get; private set; }
        public string Post { get; private set; }
        public string Country { get; private set; }

        public AddressCorrespondenceBasket(string address, string city, PostCodeBasket postCode, string post, string country)
        {
            Adres = address;
            City = city;
            PostCode = postCode;
            Post = post;
            Country = country;
        }

        protected AddressCorrespondenceBasket()
        {
            
        }
    }
    
    public class PostCodeBasket : ValueObject
    {
        public string Code { get; private set; }

        public PostCodeBasket(string code)
        {
            Code = code;
        }
    }
}