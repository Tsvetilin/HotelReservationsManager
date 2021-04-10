using Data.Seeders;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data
{
    /// <summary>
    /// Default application database seeder
    /// </summary>
    public class ApplicationDbContextSeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            var seeders = new List<ISeeder>
            {
                new RolesSeeder(),
                new UsersSeeder(),
                new SettingsSeeder(),
                new RoomsSeeder(),
                new ReservationsSeeder(),
            };

            foreach (var seeder in seeders)
            {
                await seeder.SeedAsync(dbContext, logger);
            }

            logger.LogInformation("Finished executing seeders");
        }
    }
}
