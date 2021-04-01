using Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Common;
using Services.Mapping;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Reservations;
using Web.Models.Rooms;

namespace Web.Controllers
{
    public class ReservarionsController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IUserService userService;
        private readonly IRoomService roomService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservarionsController(IReservationService reservationService,
                                      IUserService userService, 
                                      IRoomService roomService,
                                      UserManager<ApplicationUser> userManager)
        {
            this.reservationService = reservationService;
            this.userService = userService;
            this.roomService = roomService;
            this.userManager = userManager;
        }


        //[Authorize]
        public async Task<IActionResult> Index(int id = 0, int elementsOnPage=10)
        {
            var user = await userManager.GetUserAsync(User);
            var reservations = await reservationService.GetReservationsForUser<ReservationViewModel>(user.Id);


            int pageCount = (int)Math.Ceiling((double)reservations.Count() / elementsOnPage);
            if (id > pageCount || id < 1)
            {
                id = 1;
            }

            var viewModel = new ReservationsIndexViewModel
            {
                CurrentPage = id,
                PagesCount = pageCount,
                Reservations = reservations.GetPageItems(id, elementsOnPage),
            };
            
            return this.View(viewModel);
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.reservationService.GetReservation<ReservationViewModel>(id);
            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(string id)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if(room==null || (room?.IsTaken??true))
            {
                return this.NotFound();
            }

            var inputModel = new ReservationInputModel
            {
                RoomId = room.Id,
                Reservations = room.Reservations,
                RoomCapacity=room.Capacity,
                AllInclusivePrice=000,
                RoomAdultPrice=room.AdultPrice,
                BreakfastPrice=000,
                RoomChildrenPrice=room.ChildrenPrice,
                RoomType=room.Type,
            };

            return this.View(inputModel);
        }


        public async Task<IActionResult> Create(string id, ReservationInputModel inputModel)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room == null || (room?.IsTaken ?? true))
            {
                return this.NotFound();
            }

            // TODO: Check if room is empty in the selected time, if atlest 1 person is going, calculate price
            //       Pass room properties again if view is returned

            /*
            var inputModel = new ReservationInputModel
            {
                RoomId = room.Id,
                Reservations = room.Reservations,
                RoomCapacity = room.Capacity,
                AllInclusivePrice = 000,
                RoomAdultPrice = room.AdultPrice,
                BreakfastPrice = 000,
                RoomChildrenPrice = room.ChildrenPrice,
                RoomType = room.Type,
            };*/

            return this.View(inputModel);
        }

        // TODO: Concurrancy concerns


        public async Task<IActionResult> Update(string id)
        {
            //Same as create, but have to exclude the particular date periods and sat they are free

            var reservation = await reservationService.GetReservation<ReservationInputModel>(id);

            if (reservation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return this.View(reservation);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ReservationInputModel inputModel)
        {
            var reservation = await reservationService.GetReservation<ReservationInputModel>(id);

            if (reservation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (!ModelState.IsValid)
            {
                return this.View(inputModel);
            }

            var clientsData = inputModel.ClientData.AsQueryable().ProjectTo<ClientData>().ToList();
            await reservationService.UpdateReservation(id, inputModel.Price, inputModel.AccommodationDate, inputModel.ReleaseDate, inputModel.AllInclusive, inputModel.Breakfast,clientsData);

            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var reservation = await reservationService.GetReservation<ReservationInputModel>(id);

            if (reservation == null)
            {
                return RedirectToAction(nameof(Index));
            }

            await reservationService.DeleteReservation(id);

            return this.View();
        }
    }
}