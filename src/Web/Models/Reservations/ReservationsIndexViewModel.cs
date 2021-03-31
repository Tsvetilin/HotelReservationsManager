using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.ViewModels;

namespace Web.Models.Reservations
{
    public class ReservationsIndexViewModel : PageViewModel
    {
        public IEnumerable<ReservationViewModel> Reservations { get; set; }
    }
}
