using System.Collections.Generic;

namespace Search.API.Contracts.Responses
{
    public record AggregationDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public IReadOnlyCollection<BucketDto> Buckets { get; set; }
    }
}