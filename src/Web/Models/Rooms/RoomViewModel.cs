using Data.Enums;
using System.Collections.Generic;

namespace Web.Models.Rooms
{
    public class RoomViewModel
    {
        public string Id { get; set; }
        public int Number { get;set}
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
        public bool IsTaken { get; set; }
        public double AdultPrice { get; set; }
        public double ChildrenPrice { get; set; }
        public IEnumerable<ReservationPeriod> Reservations {get;set;}

    }
}
