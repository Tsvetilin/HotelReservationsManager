using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models.ViewModels
{
    class EmployeeDataViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public bool IsActive { get; set; }
    }
}
