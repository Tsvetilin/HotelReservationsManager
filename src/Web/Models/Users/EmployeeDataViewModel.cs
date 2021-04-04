using System;

namespace Web.Models.ViewModels
{
    public class EmployeeDataViewModel
    {
        public string UserId { get; set; }
        public string UserUserName { get; set; }
        public string UserFirstName { get; set; }
        public string SecondName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public string UserPhoneNumber { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public bool IsActive { get; set; }
    }
}
