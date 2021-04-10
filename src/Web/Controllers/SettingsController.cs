using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Web.Common;
using System.Threading.Tasks;
using Web.Models.Settings;
using Services.Data;

namespace Web.Controllers
{
    [Authorize(Roles ="Admin")]
    public class SettingsController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly ISettingService settingService;

        public SettingsController(IMemoryCache memoryCache, ISettingService settingService)
        {
            this.memoryCache = memoryCache;
            this.settingService = settingService;
        }

        public async Task<IActionResult> Index()
        {
            var model = new SettingsIndexModel
            {
                AllInclusivePrice = await memoryCache.GetAllInclusivePrice(settingService),
                BreakfastPrice = await memoryCache.GetBreakfastPrice(settingService),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(SettingsIndexModel model)
        {
            await settingService.UpdateAsync("AllInclusivePrice", 
                                             model.AllInclusivePrice.ToString(),
                                             typeof(double).ToString());
            await settingService.UpdateAsync("BreakfastPrice", 
                                             model.BreakfastPrice.ToString(),
                                             typeof(double).ToString());

            memoryCache.ClearPriceCache();

            TempData["Success"] = true;

            return RedirectToAction(nameof(Index));
        }

    }
}
