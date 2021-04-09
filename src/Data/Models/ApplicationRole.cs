using Microsoft.AspNetCore.Identity;
using System;

/// <summary>
/// Model layer related to context data models definition
/// </summary>
namespace Data.Models
{
    /// <summary>
    /// Default application roles scheme
    /// </summary>
    public class ApplicationRole : IdentityRole<string>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }
    }
}
