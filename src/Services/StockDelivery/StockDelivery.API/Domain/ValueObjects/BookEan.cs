using StockDelivery.API.Domain.Common;

namespace StockDelivery.API.Domain.ValueObjects
{
    public class BookEan : ValueObject
    {
        public string Ean { get; private set; }

        //TODO pass to constructor instance of command that came from Book service
        public BookEan(string ean)
        {
            Ean = ean;
        }
    }
}