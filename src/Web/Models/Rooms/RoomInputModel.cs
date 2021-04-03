using Data.Enums;

namespace Web.Models.InputModels
{
    public class RoomInputModel
    {
        public string Number { get; set; }
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
        public double AdultPrice { get; set; }
        public double ChildrenPrice { get; set; }
        
    }
}
