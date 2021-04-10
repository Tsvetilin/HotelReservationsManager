using System;
using System.ComponentModel.DataAnnotations;
using Web.Common;

namespace Web.Models.InputModels
{
    public class EmployeeInputModel
    {
        [MaxLength(50)]
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }
       
        [MaxLength(50)]
        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        
        [MaxLength(50)]
        [Required]
        [Display(Name = "Second name")]
        public string SecondName { get; set; }
        
        [MaxLength(50)]
        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        
        [Required]    
        [UCNValidator]
        public string UCN { get; set; }
        public DateTime DateOfAppointment { get; set; }
       
        [Required]
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Is Adult")]
        public bool IsAdult { get; set; }
    }
}
