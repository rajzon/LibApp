using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain.ValueObjects
{
    public class StockId : ValueObject
    {
        public string Id { get; private set; }

        public StockId()
        {
            //TODO: Add solution for generating Stock 
            Id = "Id from function that generates StockId";
        }
    }
}