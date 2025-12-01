using Microsoft.EntityFrameworkCore;

namespace Inventory.BLL.Services.PaginationHelper
{
    public static class Pagination
    {
        public static PagedResult<T> ToPagedResult<T>(this IQueryable<T> query, int page, int pageSize)
        where T : class
        {
            var totalCount =  query.Count();
            var itInventory =  query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                ItInventory = itInventory,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };
        }

        public static PagedResult<T> ToPagedResult<T>(this IEnumerable<T> source, int page, int pageSize)
        {
            var totalCount = source.Count();

            var itInventory = source
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PagedResult<T>
            {
                ItInventory = itInventory,
                TotalCount = totalCount,
                PageSize = pageSize,
                CurrentPage = page
            };

        }
    
    }


}
