using System.Collections.Generic;
using Web.Models.Rooms;

namespace Web.Models.ViewModels
{
    public class RoomIndexViewModel:PageViewModel
    {
        public IEnumerable<RoomViewModel> Rooms { get; set; }
    }
}
