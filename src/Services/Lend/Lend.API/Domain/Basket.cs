using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using Lend.API.Domain.Common;

namespace Lend.API.Domain
{
    public class Basket
    {
        public CustomerBasket Customer { get; private set; }
        public IEnumerable<StockWithBooksBasket> StockWithBooks { get; private set; }

        public Basket(CustomerBasket customer, IEnumerable<StockWithBooksBasket> stockWithBooks)
        {
            Customer = customer;
            StockWithBooks = stockWithBooks?? new List<StockWithBooksBasket>();
        }

        public void AssignNewCustomer(CustomerBasket customer)
        {
            if (customer is null)
                throw new ArgumentException("Requested customer cannot be null");
            
            Customer = customer;
        }
        
        public int GetNumberOfStocks()
        {
            return StockWithBooks.Count();
        }
    }

    public class StockWithBooksBasket
    {
        public int StockId { get; private set; }
        public string Title { get;  private set; }
        public string Ean13 { get; private set; }
        public string Isbn10 { get; private set; }
        public string Isbn13 { get; private set; }
        public DateTime PublishedDate { get; private set; }
        public DateTime ReturnDate { get; private set; }
        
        public IEnumerable<CategoryBasket> Categories { get; private set; }
        public IEnumerable<AuthorBasket> Authors { get; private set; }
        public PublisherBasket Publisher { get; private set; }
        public ImageBasket Image { get; private set; }

        public StockWithBooksBasket(int stockId, string title, string ean13,
            string isbn10, string isbn13, DateTime publishedDate, DateTime returnDate, 
            IEnumerable<CategoryBasket> categories, IEnumerable<AuthorBasket> authors, PublisherBasket publisher, ImageBasket image)
        {
            StockId = stockId;
            Title = title;
            Ean13 = ean13;
            Isbn10 = isbn10;
            Isbn13 = isbn13;
            Categories = categories;
            Authors = authors;
            Publisher = publisher;
            Image = image;
            
            PublishedDate = publishedDate;
            ReturnDate = returnDate;
        }
    }
    
    public class CategoryBasket
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public CategoryBasket(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    
    
    public class AuthorBasket
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string FullName { get; private set; }

        public AuthorBasket(int id, string firstName, string lastName, string fullName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            FullName = fullName;
        }
    }
    
    
    public class ImageBasket
    {
        public string Url { get; private set; }
        public bool IsMain { get; private set; }

        public ImageBasket(string url, bool isMain)
        {
            Url = url;
            IsMain = isMain;
        }
    }
    
    public class PublisherBasket
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public PublisherBasket(int id, string name)
        {
            Id = id;
            Name = name;
        }
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
        public AddressCorrespondenceBasket CorrespondenceAddress { get; private set; }

        public CustomerBasket(int id, string name, string surname, EmailBasket email,
            IdCardBasket personIdCard, IdentityType identityType, string nationality,
            DateTime dateOfBirth, AddressBasket address, AddressCorrespondenceBasket correspondenceAddress)
        {
            Id = id;
            Name = name;
            Surname = surname;
            Email = email;
            PersonIdCard = personIdCard;
            IdentityType = identityType;
            Nationality = nationality;
            DateOfBirth = dateOfBirth;
            Address = address;
            CorrespondenceAddress = correspondenceAddress;
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