namespace StockDelivery.API.Commands.V1.Dtos
{
    public class ActiveDeliveryItemForEditDto
    {
        public int ItemId { get; init; }
        public int BookId { get; init; }
        public string BookEan { get; init; }
        public short ItemsCount { get; init; }
    }
}