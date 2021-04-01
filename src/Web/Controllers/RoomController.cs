using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.InputModels;
using Services;
using Data;
using Data.Models;
using Web.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Web.Models.Rooms;

namespace Web.Controllers
{
    public class RoomController : Controller
    {
        private readonly IRoomService roomService;
        private readonly ApplicationDbContext context;      
        public RoomController(IRoomService _roomService)
        {
            roomService = _roomService;
        }
        public async Task<IActionResult> Index(int pageSize = 10)
        {
            var model = new RoomIndexViewModel();
            model.Pager ??= new PageViewModel();
            model.Pager.CurrentPage = model.Pager.CurrentPage <= 0 ? 1 : model.Pager.CurrentPage;

            List<RoomViewModel> rooms = await context.Rooms.Skip((model.Pager.CurrentPage - 1) * pageSize).Take(pageSize).Select(x => new RoomViewModel()
            {
                Id = x.Id,
                Capacity = x.Capacity,
                AdultPrice = x.AdultPrice,
                ChildrenPrice = x.ChildrenPrice,
                Reservations = (IEnumerable<ReservationPeriod>)x.Reservations,
                IsTaken = x.IsTaken,
                Type = x.Type

            }).ToListAsync();

            model.Rooms = rooms;
            model.Pager.PagesCount = (int)Math.Ceiling(await context.Rooms.CountAsync() / (double)pageSize);

            return View(model);
        }
        [HttpPost]      
        public async Task<IActionResult> Add(RoomInputModel createModel)
        {
            if (ModelState.IsValid)
            {
                Room room = new Room
                {
                    Id = createModel.Id,
                    Capacity = createModel.Capacity,
                    AdultPrice = createModel.AdultPrice,
                    ChildrenPrice = createModel.ChildrenPrice,
                    Reservations = createModel.Reservations,
                    IsTaken = createModel.IsTaken,
                    Type = createModel.Type
                };

                context.Add(room);
                await context.SaveChangesAsync();

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

        //public async Task<IActionResult> Update(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Room room = await context.Rooms.FindAsync(id);
        //    if (room == null)
        //    {
        //        return NotFound();
        //    }

        //    return RedirectToAction("Index", "Rooms");
        //}
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Update(string id, RoomInputModel input)
        {

            var uRoom = roomService.GetRoom<RoomInputModel>(id);
            if(ModelState.IsValid)
            {
                if(uRoom==null)
                {
                    return RedirectToAction(nameof(Index));
                }
                var room = new Room
                {
                    Id=input.Id,
                    Capacity=input.Capacity,
                    AdultPrice = input.AdultPrice,
                    ChildrenPrice = input.ChildrenPrice,
                    Reservations = input.Reservations,
                    IsTaken = input.IsTaken,
                    Type = input.Type

                };
                    
                await roomService.UpdateRoom(id,room);
                return RedirectToAction("Index", "Rooms");
            }
            return this.View();
        }   
    } 
}




