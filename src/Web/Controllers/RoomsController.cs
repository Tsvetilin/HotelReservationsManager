using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Data.Models;
using Web.Models.ViewModels;
using Web.Models.Rooms;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using Services.Data;
using Web.Common;
using Data.Enums;
using Services.Common;
using Microsoft.AspNetCore.Http;
using System.IO;
using Services.External;

namespace Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService roomService;
        private readonly IMemoryCache memoryCache;
        private readonly ISettingService settingService;
        private readonly IImageManager imageManager;

        public RoomsController(IRoomService _roomService,
                               IMemoryCache memoryCache,
                               ISettingService settingService,
                               IImageManager imageManager)
        {
            roomService = _roomService;
            this.memoryCache = memoryCache;
            this.settingService = settingService;
            this.imageManager = imageManager;
        }
        public async Task<IActionResult> Index(int id = 1, 
                                               int pageSize = 10, 
                                               bool availableOnly = false, 
                                               RoomType[] type = null,
                                               int minCapacity = 0)
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
            return this.NotFound();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(RoomInputModel createModel)
        {

            if (!await roomService.IsRoomNumberFree(createModel.Number))
            {
                ModelState.AddModelError(nameof(createModel.Number), "Room with this number alreay exists");
            }

            if (createModel.UseSamePhoto)
            {
                ModelState.AddModelError("Error", "Error parsing your request");
            }
            else if (createModel.PhotoUpload != null)
            {
                var timestamp = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}";
                var fileName = $"_{timestamp}_HMS_RoomPhoto";

                IFormFile file = createModel.PhotoUpload;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var photoUrl = await imageManager.UploadImageAsync(stream, fileName);

                if (string.IsNullOrWhiteSpace(photoUrl) ||  photoUrl.StartsWith("Error"))
                {
                    ModelState.AddModelError(nameof(createModel.PhotoUpload), $"An error occured: {photoUrl}.");
                    return this.View(createModel);
                }

                var room = new Room
                {
                    Capacity = createModel.Capacity,
                    AdultPrice = createModel.AdultPrice,
                    ChildrenPrice = createModel.ChildrenPrice,
                    Type = createModel.Type,
                    Number = createModel.Number,
                    ImageUrl = photoUrl,
                };

                await roomService.AddRoom(room);

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError(nameof(createModel.PhotoUpload), "Image is required. Upload one.");
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
                return this.RedirectToAction("Index", "Rooms");
            }
            return this.NotFound();
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

            var uRoom = await roomService.GetRoom<RoomInputModel>(id);
            if (uRoom == null)
            {
                return this.NotFound();
            }

            if (!await roomService.IsRoomNumberFree(input.Number, id))
            {
                ModelState.AddModelError(nameof(input.Number), "Number with same Id already exists");
            }

            if (!ModelState.IsValid)
            {
                return this.View(input);
            }

            string photoUrl = string.Empty;
           
            if (input.UseSamePhoto)
            {
                photoUrl = (await roomService.GetRoom<RoomViewModel>(id)).ImageUrl;
            }
            else if (input.PhotoUpload != null)
            {
                var timestamp = $"{DateTime.Today.Day}-{DateTime.Today.Month}-{DateTime.Today.Year}";
                var fileName = $"_{timestamp}_HMS_RoomPhoto";

                IFormFile file = input.PhotoUpload;

                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                photoUrl = await imageManager.UploadImageAsync(stream, fileName);

                if (string.IsNullOrWhiteSpace(photoUrl)|| photoUrl.StartsWith("Error"))
                {
                    ModelState.AddModelError(nameof(input.PhotoUpload), $"An error occured: {photoUrl}.");
                    return this.View(input);
                }
            }
            else
            {
                return this.View(input);
            }

            var room = new Room
            {
                Capacity = input.Capacity,
                AdultPrice = input.AdultPrice,
                ChildrenPrice = input.ChildrenPrice,
                Type = input.Type,
                Number = input.Number,
                ImageUrl = photoUrl,
            };

            await roomService.UpdateRoom(id,room);

            return RedirectToAction(nameof(Index));
        }
    }
}
