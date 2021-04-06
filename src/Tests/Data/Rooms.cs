

using Data.Enums;
using Data.Models;

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
            IsTaken = IsTaken1
        };
        public static readonly Room Room2 = new Room
        {
            AdultPrice = AdultPrice2,
            ChildrenPrice = ChildrenPrice2,
            Capacity = Capacity2,
            Number = 2,
            Type = RoomType.Penthouse,
            IsTaken = IsTaken2
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
