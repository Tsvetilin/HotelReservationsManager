using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Services;
using Data.Models;
using Web.Models.ViewModels;
using Web.Models.Rooms;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Services.Data;
using Web.Common;
using Data.Enums;
using Services.Common;

namespace Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService roomService;
        private readonly IMemoryCache memoryCache;
        private readonly ISettingService settingService;

        public RoomsController(IRoomService _roomService, IMemoryCache memoryCache, ISettingService settingService)
        {
            roomService = _roomService;
            this.memoryCache = memoryCache;
            this.settingService = settingService;
        }
        public async Task<IActionResult> Index(int id = 1, int pageSize = 10, bool availableOnly = false, RoomType[] type = null, int minCapacity = 0)
        {
            var searchResults = await roomService.GetSearchResults<RoomViewModel>(availableOnly, type, minCapacity);
            var resultsCount = searchResults.Count();
            if (pageSize <= 0)
            {
                pageSize = 10;
            }
            var pages = (int)Math.Ceiling((double)resultsCount / pageSize);
            if (id <= 0 || id > pages)
            {
                id = 1;
            }

            var model = new RoomIndexViewModel
            {
                PagesCount = pages,
                CurrentPage = id,
                Rooms = searchResults.GetPageItems(id, pageSize),
                Controller = "Rooms",
                Action = nameof(Index),
                BreakfastPrice = await memoryCache.GetBreakfastPrice(settingService),
                AllInclusivePrice = await memoryCache.GetAllInclusivePrice(settingService),
                MaxCapacity = await roomService.GetMaxCapacity(),
                AvailableOnly = availableOnly,
                MinCapacity = minCapacity,
                Types = type,
            };

            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        public async Task<IActionResult> Details(string id)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room != null)
            {
                return this.View(room);
            }
            return this.RedirectToAction("Index", "Rooms");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(RoomInputModel createModel)
        {
            if (ModelState.IsValid)
            {
                if(!await roomService.IsRoomNumerFree(createModel.Number))
                {
                    ModelState.AddModelError(nameof(createModel.Number), "Room with this number alreay exists");
                    return this.View(createModel);
                }

                var room = new Room
                {
                    Capacity = createModel.Capacity,
                    AdultPrice = createModel.AdultPrice,
                    ChildrenPrice = createModel.ChildrenPrice,
                    Type = createModel.Type,
                    Number = createModel.Number,
                };

                await roomService.AddRoom(room);

                return RedirectToAction(nameof(Index));
            }

            return this.View(createModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room != null)
            {
                await roomService.DeleteRoom(id);
            }
            return this.RedirectToAction("Index", "Rooms");
        }

        [Authorize]
        public async Task<IActionResult> Update(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await roomService.GetRoom<RoomInputModel>(id);
            if (room == null)
            {
                return NotFound();
            }

            return this.View(room);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(string id, RoomInputModel input)
        {

            var uRoom = roomService.GetRoom<RoomInputModel>(id);
            if (ModelState.IsValid)
            {

                if (!await roomService.IsRoomNumerFree(input.Number) || uRoom == null)
                {
                    return RedirectToAction(nameof(Index));
                }

                var room = new Room
                {
                    Id = id,
                    Capacity = input.Capacity,
                    AdultPrice = input.AdultPrice,
                    ChildrenPrice = input.ChildrenPrice,
                    Type = input.Type,
                    Number = input.Number,
                };


                await roomService.UpdateRoom(id, room);
                return RedirectToAction("Index", "Rooms");

            }

            return this.View(input);
        }
    }
}
