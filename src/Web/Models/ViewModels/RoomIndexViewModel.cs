using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Models.Rooms;

namespace Web.Models.ViewModels
{
    public class RoomIndexViewModel:RoomViewModel
    {
        public PageViewModel Pager { get; set; }
        public ICollection<RoomViewModel> Rooms { get; set; }
    }
}
