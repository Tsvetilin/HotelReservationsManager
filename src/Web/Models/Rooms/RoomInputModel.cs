using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.InputModels
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
