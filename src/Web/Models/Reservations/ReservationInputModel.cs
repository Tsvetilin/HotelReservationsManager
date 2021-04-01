using Data.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [BindNever]
        public double BreakfastPrice { get; set; }
        [BindNever]
        public double AllInclusivePrice { get; set; }
        [BindNever]
        public double Price { get; set; }
        [BindNever]
        public string RoomId { get; set; }
        [BindNever]
        public IEnumerable<ReservationPeriod> Reservations { get; set; }
        [BindNever]
        public int RoomCapacity { get; set; }
        [BindNever]
        public RoomType RoomType { get; set; }
        [BindNever]
        public double RoomAdultPrice { get; set; }
        [BindNever]
        public double RoomChildrenPrice { get; set; }
    }
}
