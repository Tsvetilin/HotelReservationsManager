using Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    /// <summary>
    /// Roles table seeder
    /// </summary>
    class RolesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if(dbContext.Roles.Any())
            {
                return;
            }

            var rolesNames = new List<string>
            {
                "Admin","Employee","User"
            };

            foreach (var role in rolesNames)
            {
                dbContext.Roles.Add(new ApplicationRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper(),
                    ConcurrencyStamp = DateTime.UtcNow.Ticks.ToString()
                });
            }

            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(RolesSeeder)}");
        }
    }
}
