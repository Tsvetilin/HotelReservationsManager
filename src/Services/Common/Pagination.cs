using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Controller layer related to commonly used logic
/// </summary>
namespace Services.Common
{
    public static class Pagination
    {
        /// <summary>
        /// Get elements for page according to pagination rules specified
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection</typeparam>
        /// <param name="items">The collection to extract page items from</param>
        /// <param name="page">The number of current page</param>
        /// <param name="elementsOnPage">The count of items on every page</param>
        /// <returns>Collection of elements of type <typeparamref name="T"/> for page <paramref name="page"/></returns>
        public static IEnumerable<T> GetPageItems<T>(this IEnumerable<T> items, int page, int elementsOnPage)
        {
            return items.Skip(elementsOnPage * (page - 1)).Take(elementsOnPage).ToList();
        }

        /// <summary>
        /// Get elements for page according to pagination rules specified
        /// </summary>
        /// <typeparam name="T">The type of the objects in the collection</typeparam>
        /// <param name="itemsTask">The task containing the result collection to extract page items from</param>
        /// <param name="page">The number of current page</param>
        /// <param name="elementsOnPage">The count of items on every page</param>
        /// <returns>Collection of elements of type <typeparamref name="T"/> for page <paramref name="page"/></returns>
        public static async Task<IEnumerable<T>> GetPageItems<T>(this Task<IEnumerable<T>> itemsTask, int page, int elementsOnPage)
        {
            var items = await itemsTask;
            return items.Skip(elementsOnPage * (page - 1)).Take(elementsOnPage).ToList();
        }
    }
}
