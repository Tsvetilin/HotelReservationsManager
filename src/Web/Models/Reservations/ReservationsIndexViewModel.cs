using System.Collections.Generic;
using Web.Models.ViewModels;

namespace Web.Models.Reservations
{
    public class ReservationsIndexViewModel : PageViewModel
    {
        public IEnumerable<ReservationViewModel> Reservations { get; set; }
    }
}
