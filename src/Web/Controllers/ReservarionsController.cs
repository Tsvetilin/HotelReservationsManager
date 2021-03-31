using Microsoft.AspNetCore.Mvc;
using Services;
using System.Threading.Tasks;
using Web.Models.Reservations;

namespace Web.Controllers
{
    public class ReservarionsController : Controller
    {
        public ReservarionsController(IReservationService reservationService)
        {
            ReservationService = reservationService;
        }

        public IReservationService ReservationService { get; }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Details (string id)
        {
            return this.View();
        }

        public async Task<IActionResult> Update(string id)
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, ReservationInputModel inputModel)
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            return this.View();
        }
    }
}
