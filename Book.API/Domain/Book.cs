using System;
using System.Collections.Generic;
using System.Linq;
using Book.API.Domain.Common;

namespace Book.API.Domain
{
    public class Book : Entity, IAggregateRoot
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public BookEan13 Ean13 { get; private set; }
        public BookIsbn10 Isbn10 { get; private set; }
        public BookIsbn13 Isbn13 { get; private set; }
        public ushort PageCount { get; private set; }
        public bool Visibility { get; private set; }
        // public List<string> Images { get; private set; }
        
        // public Language Language { get; private set; }
        // public Author Author { get; private set; }
        // public Publisher Publisher { get; private set; }
        // public List<string> Categories { get; private set; }
        
        public DateTime PublishedDate { get; private set; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }

        protected Book() { }
        
        
        public Book(string title, BookEan13 ean13, string description = default, BookIsbn10 isbn10 = default, BookIsbn13 isbn13 = default, ushort pageCount = default, bool visibility = default, DateTime publishedDate = default)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty or whitespace");
            if (ean13 is null)
                throw new ArgumentException("EAN13 cannot be null");

            Title = title;
            Description = description;
            Ean13 = ean13;
            Isbn10 = isbn10;
            Isbn13 = isbn13;
            PageCount = pageCount;
            Visibility = visibility;
            PublishedDate = publishedDate;
            
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
        

        public void GenerateEANCode()
        {
            //validate input
        }
    }

    public class BookIsbn13 : ValueObject
    {
        public string Code { get; private set; }
        
        protected BookIsbn13() { }
        public BookIsbn13(string code)
        {
            if (code?.Length < 10)
                throw new ArgumentException("ISBN13 must not exceed 13 digits");
            if (! (code ?? string.Empty).All(char.IsDigit))
                throw new ArgumentException("ISBN13 must contain only digits");
            
            Code = code;
        }

    }


    public class Language : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }

        public Language(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Language name cannot be empty or whitespace");
            
            if (! name.All(char.IsLetter))
                throw new ArgumentException("Language name must contain only letters");
            
            Name = name;
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
    }

    public class BookIsbn10 : ValueObject
    {
        public string Code { get; private set; }
        
        protected BookIsbn10() { }
        public BookIsbn10(string code = null)
        {
            if (code?.Length < 10)
                throw new ArgumentException("ISBN10 must not exceed 10 digits");
            if (! (code ?? string.Empty).All(char.IsDigit))
                throw new ArgumentException("ISBN10 must contain only digits");


            Code = code;
        }
    }
}