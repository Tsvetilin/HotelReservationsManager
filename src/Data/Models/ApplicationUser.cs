using Microsoft.AspNetCore.Identity;
using System;

namespace Data.Models
{
    /// <summary>
    /// Default application user
    /// </summary>
    public class ApplicationUser : IdentityUser<string>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdult { get; set; }

        public virtual EmployeeData EmployeeData {get;set;}
    }
}
