using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModels
{
    public class UserIndexViewModel: PageViewModel
    {
        public List<ApplicationUser> Users { get; set; }
    }
}
