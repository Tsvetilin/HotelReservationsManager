using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Web.Models;
using Web.Models.Rooms;
using Web.Models.ViewModels;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        public IRoomService roomService { get; }

        public HomeController(IRoomService roomService)
        {
            this.roomService = roomService;
        }

        public async Task<IActionResult> Index(int id = 1, int pageSize = 10)
        {
            int pageCount = (int)Math.Ceiling((double)roomService.CountAllRooms() / pageSize);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            RoomIndexViewModel viewModel = new()
            {
                PagesCount = pageCount,
                CurrentPage = id,
                Rooms = await roomService.GetAllFreeRoomsAtPresent<RoomViewModel>().GetPageItems(id, pageSize),
                Controller = "Home",
                Action = nameof(Index),
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
