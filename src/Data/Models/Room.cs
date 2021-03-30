using System;
using System.Collections.Generic;
using System.Text;
using Data.Enums;

namespace Data.Models
{
    public class Room
    {
        public Room()
        {
            this.Id = Guid.NewGuid().ToString();         
        }

        public string Id { get; set; }
        public int Capacity { get; set; }
        public RoomType Type { get; set; }
        public bool IsTaken { get; set; }
        public double AdultPrice { get; set; }
        public double ChildrenPrice { get; set; }
    }
}
