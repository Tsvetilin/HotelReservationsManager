using System.Collections.Generic;

namespace Web.Models.ViewModels
{
    public class UserIndexViewModel: PageViewModel
    {
        public List<UserDataViewModel> Users { get; set; }
    }
}
