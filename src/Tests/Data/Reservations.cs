using Data.Models;
using System;
using System.Collections.Generic;

namespace Tests.Data
{
    public static class Reservations
    {          
        public static readonly Reservation Reservation1User3Room1NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(2),
            ReleaseDate = DateTime.Today.AddDays(5),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Room = Rooms.Room1,
            User = Users.User3NotEmployee
        };

        public static readonly Reservation Reservation2User4Room2NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(8),
            ReleaseDate = DateTime.Today.AddDays(10),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Room = Rooms.Room2,
            User = Users.User4NotEmployee
        };

        public static readonly Reservation Reservation3User4Room2NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(11),
            ReleaseDate = DateTime.Today.AddDays(15),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Room = Rooms.Room2,
            User = Users.User4NotEmployee
        };

        public static readonly Reservation Reservation4User4Room2NoClient = new Reservation
        {
            AccommodationDate = DateTime.Today.AddDays(12),
            ReleaseDate = DateTime.Today.AddDays(14),
            AllInclusive = AllInClusive1,
            Breakfast = Breakfast1,
            Clients = new List<ClientData>(),
            Room = Rooms.Room2,
            User = Users.User4NotEmployee
        };

        public const bool AllInClusive1 = true;
        public const bool Breakfast1 = true;

        public const bool UpdateAllInClusive1 = false;
        public const bool UpdateBreakfast1 = false;
    }
}
