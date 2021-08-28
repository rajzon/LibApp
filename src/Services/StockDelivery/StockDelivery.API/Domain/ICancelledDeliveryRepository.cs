namespace StockDelivery.API.Domain
{
    public interface ICancelledDeliveryRepository
    {
        CancelledDelivery Add(CancelledDelivery cancelledDelivery);
        void Remove(CancelledDelivery cancelledDelivery);
    }
}