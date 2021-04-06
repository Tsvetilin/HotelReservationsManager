

using Data.Enums;
using Data.Models;
using System;
using System.Collections.Generic;
using Web.Models.Reservations;
using Web.Models.Rooms;

namespace Tests.Data
{
    public static class Rooms
    {
        public static readonly Room Room1 = new Room
        {
            AdultPrice = AdultPrice1,
            ChildrenPrice = ChildrenPrice1,
            Capacity = Capacity1,
            Number = 1,
            Type = RoomType.Apartment,
            IsTaken = IsTaken1,
            Reservations = new List<Reservation>()
        };
        public static readonly Room Room2 = new Room
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = RoomType.Penthouse,
            IsTaken = IsTaken2,
        };
        public static readonly Room Room1FreeAtPresent = new Room
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = RoomType.Penthouse,
            IsTaken = IsTaken2,
            Reservations = new List<Reservation>()
            {
                new Reservation
                {
                    AccommodationDate = DateTime.Today.AddDays(-3),
                    ReleaseDate = DateTime.Today.AddDays(-1)
                }
            }
        };
        public static readonly Room Room1TakenAtPresent = new Room
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = RoomType.Penthouse,
            IsTaken = IsTaken2,
            Reservations = new List<Reservation>()
            {
                new Reservation
                {
                    AccommodationDate = DateTime.Today,
                    ReleaseDate = DateTime.Today.AddDays(3)
                }
            }
        };
        public static readonly Room Room2TakenAtPresent = new Room
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = RoomType.Penthouse,
            IsTaken = IsTaken2,
            Reservations = new List<Reservation>()
            {
                new Reservation
                {
                    AccommodationDate = DateTime.Today.AddDays(-2),
                    ReleaseDate = DateTime.Today.AddDays(5)
                }
            }
        };
        private const double AdultPrice1 = 20;
        private const double ChildrenPrice1 = 20;
        private const int Capacity1 = 2;
        private const double AdultPrice2 = 30;
        private const double ChildrenPrice2 = 30;
        private const int Capacity2 = 4;
        private const bool IsTaken1 = false;
        private const bool IsTaken2 = true;


    }
}
