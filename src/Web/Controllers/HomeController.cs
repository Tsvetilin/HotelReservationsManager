using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Services.Common;
using Services.Data;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Web.Common;
using Web.Models;
using Web.Models.Rooms;
using Web.Models.ViewModels;

/// <summary>
/// ASP.NET 5 Controllers space
/// </summary>
namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMemoryCache memoryCache;
        private readonly ISettingService settingService;
        private readonly IReservationService reservationService;
        private readonly IRoomService roomService;

        public HomeController(IRoomService roomService,
                              IMemoryCache memoryCache,
                              ISettingService settingService,
                              IReservationService reservationService)
        {
            this.roomService = roomService;
            this.memoryCache = memoryCache;
            this.settingService = settingService;
            this.reservationService = reservationService;
        }

        public async Task<IActionResult> Index(int id = 1, int pageSize = 10)
        {
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            int pageCount = (int)Math.Ceiling((double)roomService.CountAllRooms() / pageSize);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            HomePageViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Rooms = await roomService.GetAllFreeRoomsAtPresent<RoomViewModel>().GetPageItems(id, pageSize),
                Controller = "Home",
                Action = nameof(Index),
                BreakfastPrice = await memoryCache.GetBreakfastPrice(settingService),
                AllInclusivePrice = await memoryCache.GetAllInclusivePrice(settingService),
                TotalReservationsMade = await reservationService.CountAllReservations(),
                MinPrice = await roomService.GetMinPrice(),
                MaxPrice = await roomService.GetMaxPrice(),
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
