using System;
using Nest;

namespace Search.API.Domain
{
    public class Customer
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public Email Email { get; set; }
        public IdCard PersonIdCard { get; set; }
        public IdentityType IdentityType { get; set; }
        public string Nationality { get; set; }
        public long Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        
        public Address Address { get; set; }
        public AddressCorrespondence CorrespondenceAddress { get; set; }
        
        [Completion(Analyzer = "simple", SearchAnalyzer = "simple",  MaxInputLength = 50, PreserveSeparators = true, PreservePositionIncrements = true)]
        public CompletionField NameSuggest { get; set; }
        [Completion(Analyzer = "simple",  SearchAnalyzer = "simple", MaxInputLength = 50, PreserveSeparators = true, PreservePositionIncrements = true)]
        public CompletionField SurnameSuggest { get; set; }
        
        [Completion(Analyzer = "standard",  SearchAnalyzer = "standard")]
        public CompletionField EmailSuggest { get; set; }  
        
    }

    public class Address
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public string City { get; set; }
        public PostCode PostCode { get; set; }
        public string Post { get; set; }
        public string Country { get; set; }
    }

    public class PostCode
    {
        public string Code { get; set; }
    }


    public class AddressCorrespondence
    {
        public int Id { get; set; }
        public string Adres { get; set; }
        public string City { get; set; }
        public PostCode PostCode { get; set; }
        public string Post { get; set; }
        public string Country { get; set; }
    }

    public class IdCard 
    {
        public int Id { get; set; }
        public string Value { get; set; }
    }
    
    public class Email 
    {
        public int Id { get; set; }
        public string EmailAddress { get; set; }
    }
    
    public enum IdentityType
    {
        PersonIdCard,
        Passport
    }
}