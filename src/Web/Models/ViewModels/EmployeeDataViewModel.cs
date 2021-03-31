using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModels
{
    public class EmployeeDataViewModel
    {
        public string UserId { get; set; }
        public string UserUserName { get; set; }
        public string UserFirstName { get; set; }
        public string SecondName { get; set; }
        public string UserLastName { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public bool IsActive { get; set; }
    }
}
