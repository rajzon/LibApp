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
        public ushort? PageCount { get; private set; }
        public bool Visibility { get; private set; }
        
        
        
        // public List<string> Images { get; private set; }
        
        private int? _languageId;
        public int? LanguageId => _languageId;
        
        private int _authorId;
        public int AuthorId => _authorId;
        
        private int? _publisherId;
        public int? PublisherId => _publisherId;
        
        //DDD violation: Navigation property to outside aggregate in order to create many-to-many relationship
        public IReadOnlyCollection<Category> Categories { get; private set; }

        
        
        public DateTime PublishedDate { get; private set; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }

        protected Book() { }
        
        
        
        public Book(string title,
            BookEan13 ean13,
            int authorId,
            string description = default,
            BookIsbn10 isbn10 = default,
            BookIsbn13 isbn13 = default,
            int? languageId = default,
            int? publisherId = default,
            ushort? pageCount = default,
            bool visibility = default,
            DateTime publishedDate = default,
            int? bookCategoryId = default)
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
            _languageId = languageId;
            _authorId = authorId;
            _publisherId = publisherId;
            // _bookCategoryId = bookCategoryId;

            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }
        
        //TODO consider return type
        public Book AddCategory(Category category)
        {
            // this.Categories.Add(category);
            return this;
        }
        
        
        public void GenerateEANCode()
        {
            //validate input
        }
    }

    public class Category : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        
        //DDD violation: Navigation property to outside aggregate in order to create many-to-many relationship
        public IReadOnlyCollection<Book> Books { get; private set; }
        
        public Category(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name cannot be empty or whitespace");
            
            Name = name;
        }
        
        protected Category() { }
    }

    public class BookIsbn13 : ValueObject
    {
        public string Code { get; private set; }
        
        public BookIsbn13(string code)
        {
            if (code?.Length < 10)
                throw new ArgumentException("ISBN13 must not exceed 13 digits");
            if (! (code ?? string.Empty).All(char.IsDigit))
                throw new ArgumentException("ISBN13 must contain only digits");
            
            Code = code;
        }
        
        protected BookIsbn13() { }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
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
        
        protected Language() { }
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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Code;
        }
    }
}