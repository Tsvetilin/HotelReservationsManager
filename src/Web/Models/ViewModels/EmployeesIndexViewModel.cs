using System.Collections.Generic;

namespace Web.Models.ViewModels
{
    public class EmployeesIndexViewModel: PageViewModel
    {
        public List<EmployeeDataViewModel> Employees { get; set; }
    }
}
