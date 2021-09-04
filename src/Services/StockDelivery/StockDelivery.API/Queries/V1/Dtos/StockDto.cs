namespace StockDelivery.API.Queries.V1.Dtos
{
    public class StockDto
    {
        public int Id { get; init; }
        public int BookId { get; init; }
        public string Ean { get; init; }
    }
}