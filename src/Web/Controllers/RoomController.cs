using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Models.InputModels;
using Services;
using Data.Models;
using Web.Models.ViewModels;
using Web.Models.Rooms;

namespace Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService roomService;
        public RoomsController(IRoomService _roomService)
        {
            roomService = _roomService;
        }
        public async Task<IActionResult> Index(int id = 1, int pageSize = 10)
        {
            var model = new RoomIndexViewModel();
            model.PagesCount = (int)Math.Ceiling((double)roomService.CountAllRooms() / pageSize);
            model.CurrentPage = model.CurrentPage <= 0 ? 1 : id;
            model.CurrentPage = model.CurrentPage > model.PagesCount ? model.PagesCount : model.CurrentPage;
            model.Rooms = (ICollection<RoomViewModel>)await roomService.GetPageItems<RoomViewModel>(model.CurrentPage, pageSize);

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoomInputModel createModel)
        {
            if (ModelState.IsValid)
            {
                var room = new Room
                {
                    Capacity = createModel.Capacity,
                    AdultPrice = createModel.AdultPrice,
                    ChildrenPrice = createModel.ChildrenPrice,
                    Type = createModel.Type
                };

                await roomService.AddRoom(room);

                return RedirectToAction(nameof(Index));
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var room = await roomService.GetRoom<RoomInputModel>(id);
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

            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Rooms");
        }
        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.roomService.GetRoom<RoomViewModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(string id, RoomInputModel input)
        {

            var uRoom = roomService.GetRoom<RoomInputModel>(id);
            if (ModelState.IsValid)
            {
                if (uRoom == null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var room = new Room
                {
                    Id = id,
                    Capacity = input.Capacity,
                    AdultPrice = input.AdultPrice,
                    ChildrenPrice = input.ChildrenPrice,
                    Type = input.Type

                };

                await roomService.UpdateRoom(id, room);
                return RedirectToAction("Index", "Rooms");
            }
            return this.View(input);
        }
    }
}
