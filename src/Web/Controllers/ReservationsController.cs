using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Common;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Reservations;
using Web.Models.Rooms;

namespace Web.Controllers
{
    //[Authorize]
    public class ReservationsController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IUserService userService;
        private readonly IRoomService roomService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservationsController(IReservationService reservationService,
                                      IUserService userService,
                                      IRoomService roomService,
                                      UserManager<ApplicationUser> userManager)
        {
            this.reservationService = reservationService;
            this.userService = userService;
            this.roomService = roomService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index(int id = 0, int elementsOnPage = 10)
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
            var user = await userManager.GetUserAsync(User);
            var viewModel = await this.reservationService.GetReservation<ReservationViewModel>(id);
            if (viewModel == null || user.Id != viewModel.UserId)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create(string id)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room == null || (room?.IsTaken ?? true))
            {
                return this.NotFound();
            }

            var inputModel = FillRoomData(new ReservationInputModel(), room);

            return this.View(inputModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(string id, ReservationInputModel inputModel)
        {
            var room = await roomService.GetRoom<RoomViewModel>(id);
            if (room == null || (room?.IsTaken ?? true))
            {
                return this.NotFound();
            }

            var roomIsEmpty = !room.Reservations.Any(x =>
                (x.AccommodationDate > inputModel.AccommodationDate && x.AccommodationDate < inputModel.ReleaseDate) ||
                (x.ReleaseDate > x.AccommodationDate && x.ReleaseDate < inputModel.ReleaseDate));

            if (!roomIsEmpty)
            {
                this.ModelState.AddModelError(nameof(inputModel.AccommodationDate), "Room is already reserved at that time");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(FillRoomData(inputModel, room));
            }

            var clients = new List<ClientData>();
            foreach (var client in inputModel.ClientData)
            {
                clients.Add(await this.userService.CreateClient(client.Email, client.FullName, client.IsAdult));
            }

            var user = await userManager.GetUserAsync(User);

            var reservation = await reservationService.AddReservation(
                room.Id,
                inputModel.AccommodationDate,
                inputModel.ReleaseDate,
                inputModel.AllInclusive,
                inputModel.Breakfast,
                clients,
                user);

            return this.RedirectToAction(nameof(Details), new { id = reservation.Id });
        }

        public async Task<IActionResult> Update(string id)
        {
            var user = await userManager.GetUserAsync(User);
            //TODO: Have to exclude the particular date periods and say they are free

            var reservation = await reservationService.GetReservation<ReservationInputModel>(id);
            if (reservation == null || user.Id != reservation.UserId)
            {
                return this.NotFound();
            }
            reservation.Reservations = reservation.Reservations.Where(x => !(x.AccommodationDate == reservation.AccommodationDate && x.ReleaseDate == reservation.ReleaseDate));

            var room = await roomService.GetRoom<RoomViewModel>(reservation.RoomId);

            return this.View(FillRoomData(reservation, room));
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ReservationInputModel inputModel)
        {
            var user = await userManager.GetUserAsync(User);
            var reservation = await reservationService.GetReservation<ReservationInputModel>(id);
            if (reservation == null || user.Id != reservation.UserId)
            {
                return this.NotFound();
            }

            var room = await roomService.GetRoom<RoomViewModel>(reservation.RoomId);
            reservation.Reservations = reservation.Reservations.Where(x => !(x.AccommodationDate == reservation.AccommodationDate && x.ReleaseDate == reservation.ReleaseDate));

            var roomIsEmpty = !reservation.Reservations.Any(x =>
                (x.AccommodationDate > inputModel.AccommodationDate && x.AccommodationDate < inputModel.ReleaseDate) ||
                (x.ReleaseDate > x.AccommodationDate && x.ReleaseDate < inputModel.ReleaseDate));

            if (!roomIsEmpty)
            {
                this.ModelState.AddModelError(nameof(inputModel.AccommodationDate), "Room is already reserved at that time");
            }

            if (!this.ModelState.IsValid)
            {
                return this.View(FillRoomData(inputModel, room));
            }

            var cls = inputModel.ClientData.Select(x => new ClientData
            {
                IsAdult = x.IsAdult,
                Email = x.Email,
                FullName = x.FullName,
                Id = x.Id,
            });

            var clients = await reservationService.UpdateClientsForReservation(reservation.Id, cls);

            await reservationService.UpdateReservation(
                reservation.Id,
                room.Id,
                inputModel.AccommodationDate,
                inputModel.ReleaseDate,
                inputModel.AllInclusive,
                inputModel.Breakfast,
                clients,
                user);

            return this.RedirectToAction(nameof(Details), new { id = reservation.Id });
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

        private static ReservationInputModel FillRoomData(ReservationInputModel inputModel, RoomViewModel room)
        {
            inputModel.RoomId = room.Id;
            inputModel.Reservations = room.Reservations;
            inputModel.RoomCapacity = room.Capacity;
            inputModel.AllInclusivePrice = 000;
            inputModel.RoomAdultPrice = room.AdultPrice;
            inputModel.BreakfastPrice = 000;
            inputModel.RoomChildrenPrice = room.ChildrenPrice;
            inputModel.RoomType = room.Type;

            return inputModel;
        }
    }
}