using Data.Enums;
using System;
using System.Collections.Generic;
using Web.Models.Clients;
using Web.Models.Rooms;

namespace Web.Models.Reservations
{
    public class ReservationInputModel
    {
        // Should add breakfast and all inclusive prices as well as dynamic price calculation
        public string Id { get; set; }
        public IEnumerable<ClientViewModel> ClientData { get; set; }
        public DateTime AccommodationDate { get; set; }
        public DateTime ReleaseDate { get; set; }
        public bool Breakfast { get; set; }
        public bool AllInclusive { get; set; }
        //should be passed down
        public double BreakfastPrice { get; set; }
        public double AllInclusivePrice { get; set; }
        //should be verified on submit
        public double Price { get; set; }
        public string RoomId { get; set; }
        public IEnumerable<ReservationPeriod> Reservations { get; set; }
        public int RoomCapacity { get; set; }
        public RoomType RoomType { get; set; }
        public double RoomAdultPrice { get; set; }
        public double RoomChildrenPrice { get; set; }
    }
}
