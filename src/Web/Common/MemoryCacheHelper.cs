using Microsoft.Extensions.Caching.Memory;
using Services.Data;
using System.Threading.Tasks;

namespace Web.Common
{
    /// <summary>
    /// Memory cache extension methods helper for commonly used data
    /// </summary>
    public static class MemoryCacheHelper
    {
        public async static Task<double> GetBreakfastPrice(this IMemoryCache memoryCache, ISettingService settingService)
        {
            return await memoryCache.GetPrice("BreakfastPrice", settingService);
        }

        public async static Task<double> GetAllInclusivePrice(this IMemoryCache memoryCache, ISettingService settingService)
        {
            return await memoryCache.GetPrice("AllInclusivePrice", settingService);
        }

        public async static Task<double> GetPrice(this IMemoryCache memoryCache, string key, ISettingService settingService)
        {
            if (!memoryCache.TryGetValue(key, out double price))
            {
                price = double.Parse((await settingService.GetAsync(key)).Value);
                memoryCache.Set(key, price);
            }
            return price;
        }

        public static void ClearPriceCache(this IMemoryCache memoryCache)
        {
            memoryCache.Remove("AllInclusivePrice");
            memoryCache.Remove("BreakfastPrice");
        }
    }
}
