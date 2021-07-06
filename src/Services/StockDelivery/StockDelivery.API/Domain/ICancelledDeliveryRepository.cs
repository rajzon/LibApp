namespace StockDelivery.API.Domain
{
    public interface ICancelledDeliveryRepository
    {
        CancelledDelivery Add(CancelledDelivery activeDelivery);
        void Remove(CancelledDelivery cancelledDelivery);
    }
}