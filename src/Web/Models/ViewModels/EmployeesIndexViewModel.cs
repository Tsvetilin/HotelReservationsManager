using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Models.ViewModels
{
    public class EmployeesIndexViewModel: PageViewModel
    {
        public List<EmployeeDataViewModel> Employees { get; set; }
    }
}
