using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.InputModels;
using Services;

namespace Web.Controllers
{
    public class RoomController: Controller
    {
        //private readonly IRoomService roomService;
        //public RoomController()
        //{

        //}
        //[Authorize]
        
        //public async Task<IActionResult> Add()
        //{

        //}

        //public async Task<IActionResult> Update(string id)
        //{
        //    var room = await roomService.GetRoom<RoomInputModel>(id);
        //    if (room != null)
        //    {
        //        return this.View(room);
        //    }

        //    return RedirectToAction("Index", "Rooms");
        //}  

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> Delete(string id)
        //{
        //    var room = await roomService.GetRoom<RoomInputModel>(id);
        //    if (room != null)
        //    {
        //        await roomService.DeleteRoom(id);
        //    }
        //    return this.RedirectToAction("Index", "Rooms");
        //}

        //public async Task<IActionResult> Details(int id)
        //{
        //    return this.RedirectToAction("Index", "Rooms");
        //}

    }
}
