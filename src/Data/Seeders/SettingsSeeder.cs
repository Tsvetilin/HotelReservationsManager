using Data.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    /// <summary>
    /// Settings table seeder
    /// </summary>
    public class SettingsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if (dbContext.Settings.Any())
            {
                return;
            }

            await dbContext.Settings.AddRangeAsync(new Setting[]
            {
                new Setting
                {
                    Key="AllInclusivePrice",
                    Value="100",
                    Type=typeof(double).ToString(),
                },
                new Setting
                {
                    Key="BreakfastPrice",
                    Value="50",
                    Type=typeof(double).ToString(),
                }
            });
           
            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(SettingsSeeder)}");
        }
    }
}
