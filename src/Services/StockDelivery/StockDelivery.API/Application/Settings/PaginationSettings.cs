using System.Collections.Generic;

namespace StockDelivery.API.Application.Settings
{
    public static class PaginationSettings
    {
        public static readonly short[] ALLOWED_PAGE_SIZES = new short[]{ 10, 20, 40 };
        public static readonly short DEFAULT_PAGE_SIZE = 10;
    }
}