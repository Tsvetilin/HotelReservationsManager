using System.Collections.Generic;
using Web.Models.Rooms;

namespace Web.Models.ViewModels
{
    public class RoomIndexViewModel:PageViewModel
    {
        public ICollection<RoomViewModel> Rooms { get; set; }
    }
}
