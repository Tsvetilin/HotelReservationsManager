using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public interface ISeeder
    {
        public Task SeedAsync(ApplicationDbContext dbContext, ILogger logger);
    }
}
