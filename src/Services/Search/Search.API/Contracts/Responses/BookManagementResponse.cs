using System;
using System.Collections.Generic;
using System.Linq;
using EventBus.Messages.Commands;
using Nest;
using Search.API.Domain;
using Search.API.Infrastructure.Data;

namespace Search.API.Contracts.Responses
{
    public class BookManagementResponse
    {
        public long Total { get; init; }
        public IReadOnlyCollection<BookManagementResponseDto> Results { get; init; }
        public IReadOnlyCollection<AggregationDto> Aggregations { get; init; }

        public BookManagementResponse(ISearchResponse<Book> searchResult)
        {
            Aggregations = new List<AggregationDto>()
            {
                new AggregationDto()
                {
                    Name = "categories",
                    Buckets = searchResult.Aggregations.Terms("categories")
                        .Buckets
                        .Select(bucket => new BucketDto() {Key = bucket.Key, Count = bucket.DocCount})
                        .ToList()
                },
                new AggregationDto()
                {
                    Name = "visibility",
                    Buckets = searchResult.Aggregations.Terms("visibility")
                        .Buckets
                        .Select(bucket => new BucketDto() {Key = bucket.Key, Count = bucket.DocCount})
                        .ToList()
                },
                new AggregationDto()
                {
                    Name = "authors",
                    Buckets = searchResult.Aggregations.Terms("authors")
                        .Buckets
                        .Select(bucket => new BucketDto() {Key = bucket.Key, Count = bucket.DocCount})
                        .ToList()
                },
                new AggregationDto()
                {
                    Name = "languages",
                    Buckets = searchResult.Aggregations.Terms("languages")
                        .Buckets
                        .Select(bucket => new BucketDto() {Key = bucket.Key, Count = bucket.DocCount})
                        .ToList()
                },
                new AggregationDto()
                {
                    Name = "publishers",
                    Buckets = searchResult.Aggregations.Terms("publishers")
                        .Buckets
                        .Select(bucket => new BucketDto() {Key = bucket.Key, Count = bucket.DocCount})
                        .ToList()
                }
            };
        }
    }
    

    public record BookManagementResponseDto
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Ean13 { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public ushort? PageCount { get; set; }
        public bool Visibility { get; set; }


        public IReadOnlyCollection<ImageResponseDto> Images { get; set; }
        
        public IReadOnlyCollection<CategoryResponseDto> Categories { get; set; }
        
        public LanguageResponseDto Language { get; set; }
        
        public PublisherResponseDto Publisher { get; set; }
        
        public IReadOnlyCollection<AuthorResponseDto> Authors { get; set; }


        public DateTime? PublishedDate { get; set; }
        public DateTime ModificationDate { get; set; }
        public DateTime CreationDate { get; set; }
    }

    public record ImageResponseDto
    {
        public string Url { get; set; }
        public bool IsMain { get; set; }
    }

    public record CategoryResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public record AuthorResponseDto
    {
        public int Id { get; set; }
        public AuthorNameResponseDto Name { get; set; }
    }

    public record AuthorNameResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
    }
    
    public record LanguageResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public record PublisherResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}