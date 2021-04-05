using Data.Seeders;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContextSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new UsersSeeder(),
                new SettingsSeeder(),
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, logger);
            }

            logger.LogInformation("Finished executing seeders");

            return;
        }
    }
}
