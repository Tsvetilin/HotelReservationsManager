using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Common
{
    public static class Pagination
    {
        public static IEnumerable<T> GetPageItems<T>(this IEnumerable<T> items, int page, int elementsOnPage)
        {
            return items.Skip(elementsOnPage * (page - 1)).Take(elementsOnPage).AsQueryable().ToList();
        }

        public static async Task<IEnumerable<T>> GetPageItems<T>(this Task<IEnumerable<T>> itemsTask, int page, int elementsOnPage)
        {
            var items = await itemsTask;
            return items.Skip(elementsOnPage * (page - 1)).Take(elementsOnPage).AsQueryable().ToList();
        }
    }
}
