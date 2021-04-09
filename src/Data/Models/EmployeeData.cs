using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Models
{
    /// <summary>
    /// Employees data scheme
    /// </summary>
    public class EmployeeData
    {
        [Key]
        [ForeignKey("Id")]
        public string UserId { get; set; }
        public string SecondName { get; set; }
        public string UCN { get; set; }
        public DateTime DateOfAppointment { get; set; }
        public bool IsActive { get; set; }
        public DateTime? DateOfResignation { get; set; }

        public virtual ApplicationUser User { get; set; }

    }
}
