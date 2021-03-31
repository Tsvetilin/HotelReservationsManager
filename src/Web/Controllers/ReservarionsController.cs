using Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Common;
using Services.Mapping;
using System;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Reservations;

namespace Web.Controllers
{
    public class ReservarionsController : Controller
    {
        private readonly IReservationService reservationService;
        private readonly IUserService userService;
        private readonly UserManager<ApplicationUser> userManager;

        public ReservarionsController(IReservationService reservationService, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.reservationService = reservationService;
            this.userService = userService;
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

        public async Task<IActionResult> Update(string id)
        {
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