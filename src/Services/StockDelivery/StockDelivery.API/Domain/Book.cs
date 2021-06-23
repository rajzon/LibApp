using System;
using System.Collections.Generic;
using StockDelivery.API.Domain.Common;
using StockDelivery.API.Domain.ValueObjects;

namespace StockDelivery.API.Domain
{
    //TODO: consider existence of that class, because Book wont exist in that service but instead it exist in Book Service
    public class Book : Entity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public BookEan13 Ean13 { get; private set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public ushort? PageCount { get; set; }
        public bool Visibility { get; set; }


        public IReadOnlyCollection<Image> Images { get; set; }
        
        public IReadOnlyCollection<Category> Categories { get; set; }
        
        public Language Language { get; set; }
        
        public Publisher Publisher { get; set; }
        
        public IReadOnlyCollection<Author> Authors { get; set; }


        public DateTime? PublishedDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CreationDate { get; set; }
    }
    
    
    public record Image 
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }

    public record Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public record Author
    {
        public int Id { get; set; }
        public AuthorName Name { get; set; }
    }

    public record AuthorName
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
    
    public record Language
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public record Publisher
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}