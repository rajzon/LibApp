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
        public BookEan13 Ean13 { get; }
        public BookIsbn10 Isbn10 { get; private set; }
        public BookIsbn13 Isbn13 { get; private set; }
        public ushort? PageCount { get; private set; }
        public bool Visibility { get; private set; }
        
        
        
        private readonly List<Image> _images;
        public IReadOnlyCollection<Image> Images => _images;
        
        private int? _languageId;
        public int? LanguageId => _languageId;
        
        private int? _authorId;
        public int? AuthorId => _authorId;
        
        private int? _publisherId;
        public int? PublisherId => _publisherId;
        
        //DDD violation: Navigation property to outside aggregate in order to create many-to-many relationship
        private readonly List<Category> _categories;
        public IReadOnlyCollection<Category> Categories => _categories;

        
        
        public DateTime PublishedDate { get; private set; }
        public DateTime ModificationDate { get; private set; }
        public DateTime CreationDate { get; private set; }

        
        protected Book()
        {
            // _images = new List<Image>();
        }
        
        private Book(string title,
            int? authorId,
            BookEan13 bookEan13,
            string description = default,
            BookIsbn10 isbn10 = default,
            BookIsbn13 isbn13 = default,
            int? languageId = default,
            int? publisherId = default,
            ushort? pageCount = default,
            bool visibility = default,
            DateTime publishedDate = default
        ) : this()
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty or whitespace");


            Title = title;
            Description = description;
            Isbn10 = isbn10;
            Isbn13 = isbn13;
            PageCount = pageCount;
            Visibility = visibility;
            PublishedDate = publishedDate;
            _languageId = languageId;
            _authorId = authorId;
            _publisherId = publisherId;
            
            
            _categories = new List<Category>();
            Ean13 = bookEan13;
            
            ModificationDate = DateTime.UtcNow;
            CreationDate = DateTime.UtcNow;
        }

        public Book(string title, int? authorId, string description = default, BookIsbn10 isbn10 = default, BookIsbn13 isbn13 = default,
            int? languageId = default, int? publisherId = default, ushort? pageCount = default, 
            bool visibility = default, DateTime publishedDate = default
            ) : this(title, authorId, new BookEan13(), 
            description, isbn10, isbn13, languageId, publisherId, pageCount, visibility, publishedDate)
        {
            
        }
        
        public void AddCategory(Category category)
        {
            var existingCategoryRelation = _categories.SingleOrDefault(c => c.Name.Equals(category.Name));
            
            //TODO: Potentially log info about trying to add duplicate category or throw exception    
            if (existingCategoryRelation is not null)
                return;
            
            this._categories.Add(category);
        }


        public void AddImage(string url, string publicId, bool isMain)
        {
            this._images.Add(new Image(url, publicId, _images.Any()? isMain : true));
        }
        
    }
}