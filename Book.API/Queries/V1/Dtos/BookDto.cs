using System;
using System.Collections.Generic;
using Book.API.Domain;

namespace Book.API.Queries.V1.Dtos
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Ean13 { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public ushort? PageCount { get; set; }
        public bool Visibility { get; set; }
        
        
        public int? LanguageId { get; set; }
        public int? PublisherId { get; set; }
        public IReadOnlyCollection<CategoryDto> Categories { get; set; }
        public IReadOnlyCollection<AuthorDto> Authors { get; set; }


        public DateTime? PublishedDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CreationDate { get; set; }
        
    }
}