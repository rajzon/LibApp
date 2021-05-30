namespace Search.API.Contracts.Responses
{
    public record BucketDto
    {
        public string Key { get; set; }
        public long? Count { get; set; }
    }
}