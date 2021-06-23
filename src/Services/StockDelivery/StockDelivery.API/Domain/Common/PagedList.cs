using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace StockDelivery.API.Domain.Common
{
    public class PagedList<T> : List<T>
    {
        public short CurrentPage { get; }
        public short TotalPages { get; }
        public short PageSize { get; }
        public int TotalCount { get; }

        public PagedList()
        {
        }
        
        private PagedList(List<T> items, int totalCount, short currentPage, short pageSize)
        {
            TotalCount = totalCount;
            PageSize = pageSize;
            TotalPages = (short) Math.Ceiling(totalCount / (double)pageSize);
            CurrentPage = currentPage;
            
            this.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, short currentPage, short pageSize)
        {
            var total = await source.CountAsync();
            var items = await source.Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, total, currentPage, pageSize);
        }
    }
}