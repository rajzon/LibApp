using System.Linq;
using StockDelivery.API.Application.Settings;

namespace StockDelivery.API.Domain.Common
{
    public class PaginationParams
    {
        public short CurrentPage { get; init; }
        public short PageSize { get; init; }

        public PaginationParams(short currentPage, short pageSize)
        {
            CurrentPage = currentPage;
            PageSize = PaginationSettings.ALLOWED_PAGE_SIZES.Contains(pageSize)
                ? pageSize
                : PaginationSettings.DEFAULT_PAGE_SIZE;

        }
    }
}