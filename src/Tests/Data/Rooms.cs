

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
            Type = RoomType.Apartment
        };

        private const double AdultPrice1 = 20;
        private const double ChildrenPrice1 = 20;
        private const int Capacity1 = 2;


    }
}
