using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using AutoMapper;
using EventBus.Messages.Results;
using Lend.API.Domain.Common;
using Lend.API.Domain.Strategies;
using Newtonsoft.Json;

namespace Lend.API.Domain
{
    public class Basket
    {
        public CustomerBasket Customer { get; private set; }
        public List<StockWithBooksBasket> StockWithBooks { get; private set; }

        public Basket(CustomerBasket customer, List<StockWithBooksBasket> stockWithBooks)
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

        public void AddNewStock(StockWithBooksBasket stockWithBooksBasket)
        {
            if (stockWithBooksBasket is null)
                throw new ArgumentException("Requested stock cannot be null");
            if (IsStockDuplicatedInBasket(stockWithBooksBasket))
                throw new ArgumentException("Trying to add duplicate stock for basket");
            StockWithBooks.Add(stockWithBooksBasket);
        }

        public int GetNumberOfStocks()
        {
            return StockWithBooks.Count();
        }

        public bool IsStockDuplicatedInBasket(StockWithBooksBasket stockWithBooksBasket)
        {
            if (StockWithBooks.Any(s => s.StockId == stockWithBooksBasket.StockId))
                return true;
            
            return false;
        }
        
        public bool IsBookEanDuplicatedInBasket(StockWithBooksBasket stockWithBooksBasket)
        {
            if (StockWithBooks.Any(s => s.Ean13 == stockWithBooksBasket.Ean13))
                return true;
            
            return false;
        }


        public void RemoveStock(int stockId)
        {
            var stockToRemove = StockWithBooks.FirstOrDefault(s => s.StockId == stockId);
            if (stockToRemove is null)
                throw new ArgumentException("Requested stock id not exist in basket");
            
            StockWithBooks.Remove(stockToRemove);
        }

        public async Task<(bool, StrategyError)> TryEditReturnDateOfStock(int stockId, DateTime newReturnDate,
            IEnumerable<IStrategy<SimpleIntRule>> intStrategies)
        {
            var stockToModifyIndex = StockWithBooks.FindIndex(s => s.StockId == stockId);
            var originalReturnDate = StockWithBooks[stockToModifyIndex].ReturnDate;
            StockWithBooks[stockToModifyIndex].EditReturnDate(newReturnDate);

            if (intStrategies is not null)
            {
                foreach (var intStrategy in intStrategies)
                {
                    var check = await intStrategy.IsBasketMatchStrategy(this);
                    if (!check.Item1)
                    {
                        StockWithBooks[stockToModifyIndex].EditReturnDate(originalReturnDate);
                        return (false, check.Item2);
                    }
                }
            }

            return (true, null);
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
        public DateTime ReturnDate { get; set; }
        
        public IEnumerable<CategoryBasket> Categories { get; private set; }
        public IEnumerable<AuthorBasket> Authors { get; private set; }
        public PublisherBasket Publisher { get; private set; }
        public ImageBasket Image { get; private set; }

        public StockWithBooksBasket(int stockId, string title, string ean13,
            string isbn10, string isbn13, DateTime publishedDate,  
            IEnumerable<CategoryBasket> categories, IEnumerable<AuthorBasket> authors, PublisherBasket publisher, ImageBasket image,
            IEnumerable<IStrategy<SimpleIntRule>> intStrategies, DateTime? returnDate = null)
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
            if (intStrategies is not null)
            {
                foreach (var intStrategy in intStrategies)
                {
                    if (intStrategy is MaxDaysForLendBookStrategy maxDaysForLendBookStrategy)
                    {
                        var maxAllowedReturnDateSet = maxDaysForLendBookStrategy.GetRuleInfo().Result;
                        ReturnDate = returnDate ?? DateTime.UtcNow.AddDays(maxAllowedReturnDateSet.RuleValue);
                    }
                } 
            }
            
            
        }
        public void EditReturnDate(DateTime newReturnDate)
        {
            ReturnDate = newReturnDate;
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

        public EmailBasket(string emailAddress)
        {
            //TODO check if passed email is valid
            EmailAddress = emailAddress;
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

        public AddressBasket(string adres, string city, PostCodeBasket postCode, string post, string country)
        {
            Adres = adres;
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

        public AddressCorrespondenceBasket(string adres, string city, PostCodeBasket postCode, string post, string country)
        {
            Adres = adres;
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