﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Services;
using Data.Models;
using Web.Models.ViewModels;
using Web.Models.Rooms;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Web.Controllers
{
    public class RoomsController : Controller
    {
        private readonly IRoomService roomService;
        public RoomsController(IRoomService _roomService)
        {
            roomService = _roomService;
        }
        public async Task<IActionResult> Index(string search, int id = 1, int pageSize = 10)
        {
            if (!string.IsNullOrEmpty(search))
            {
                var searchResults = await roomService.GetSearchResults<RoomViewModel>(search);

                if (searchResults.Any())
                {
                    return View(new RoomIndexViewModel
                    {
                        PagesCount = 1,
                        CurrentPage = 1,
                        Rooms = searchResults.ToList(),
                        Controller = "Rooms",
                        Action = nameof(Index),
                    });
                }
                ModelState.AddModelError("Found", "Room not found!");
            }
            var model = new RoomIndexViewModel
            {
                PagesCount = (int)Math.Ceiling((double)roomService.CountAllRooms() / pageSize),
                Controller = "Rooms",
                Action = nameof(Index),
            };

            if (id <= 0 || id > model.PagesCount)
            {
                id = 1;
            }
            model.CurrentPage = id;
            model.Rooms = (ICollection<RoomViewModel>)await roomService.GetPageItems<RoomViewModel>(model.CurrentPage, pageSize);

            return View(model);
        }

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
        public async Task<IActionResult> Create(RoomInputModel createModel)
        {
            if (ModelState.IsValid)
            {
                foreach (var _room in await roomService.GetAll<RoomInputModel>())
                {
                    if (_room.Number == createModel.Number)
                    {
                        return RedirectToAction("Index", "Rooms");
                    }
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

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room != null)
            {
                if (room.Reservations == null)
                {
                    await roomService.DeleteRoom(id);
                }
                else
                {
                    ModelState.AddModelError("", "There are still reservations made");

                    return this.RedirectToAction("Index", "Rooms");
                }
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
                foreach (var _room in await roomService.GetAll<RoomInputModel>())
                {
                    if (_room.Number == input.Number||uRoom==null)
                    {
                        return RedirectToAction(nameof(Index));
                    }
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
