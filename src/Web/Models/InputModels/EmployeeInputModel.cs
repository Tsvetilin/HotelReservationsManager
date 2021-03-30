using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.InputModels
{
    public class EmployeeInputModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string UCN { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public string Error { get; set; }
        public bool IsAdult { get; set; }
    }
}
