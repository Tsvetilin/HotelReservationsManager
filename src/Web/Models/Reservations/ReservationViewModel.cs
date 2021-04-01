using System;
using System.Collections.Generic;
using Web.Models.Clients;

namespace Web.Models.Reservations
{
    public class ReservationViewModel
    {
        public string Id { get; set; }
        public IEnumerable<ClientViewModel> Clients { get; set; }
        public DateTime AccommodationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Breakfast { get; set; }
        public bool AllInclusive { get; set; }
        public double Price { get; set; }
        public string RoomId { get; set; }
        public string UserId { get; set; }
    }
}
