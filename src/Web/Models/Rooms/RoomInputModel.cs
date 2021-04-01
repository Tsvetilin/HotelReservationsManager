using Data.Enums;

namespace Web.Models.Rooms
{
    public class RoomInputModel
    {
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
        public double AdultPrice { get; set; }
        public double ChildrenPrice { get; set; }
        public int Number { get; set; }
    }
}
