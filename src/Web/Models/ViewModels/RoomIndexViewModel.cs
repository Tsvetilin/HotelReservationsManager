using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModels
{
    public class RoomIndexViewModel:RoomDataViewModel
    {
        public PageViewModel Pager { get; set; }
        public ICollection<RoomDataViewModel> Rooms { get; set; }
    }
}
