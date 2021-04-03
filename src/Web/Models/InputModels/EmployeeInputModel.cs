using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Web.Common;

namespace Web.Models.InputModels
{
    public class EmployeeInputModel
    {
        [MaxLength(50)]
        [Required]
        [Display(Name = "Username")]
        public string UserUserName { get; set; }
       
        [MaxLength(50)]
        [Required]
        [Display(Name = "First name")]
        public string UserFirstName { get; set; }
        
        [MaxLength(50)]
        [Required]
        [Display(Name = "Second name")]
        public string SecondName { get; set; }
        
        [MaxLength(50)]
        [Required]
        [Display(Name = "Last name")]
        public string UserLastName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }
        
        [Required]    
        [UCNValidator]
        public string UCN { get; set; }
        public DateTime DateOfAppointment { get; set; }
       
        [Required]
        [Phone]
        public string UserPhoneNumber { get; set; }
        [Required]
        public bool UserIsAdult { get; set; }
        public string Еrror { get; set; }
    }
}
