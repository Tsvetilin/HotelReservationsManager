using Data.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    public class RoomsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if (dbContext.Rooms.Any())
            {
                return;
            }

            await dbContext.Rooms.AddRangeAsync(new Room[]
           {
                new Room
                {
                    Id="ExampleRoom1",
                    AdultPrice=20,
                    ChildrenPrice=10,
                    Capacity=3,
                    Number=105,
                    Type=Enums.RoomType.Apartment,
                },
                new Room
                {
                    Id="ExampleRoom2",
                    AdultPrice=30,
                    ChildrenPrice=20,
                    Capacity=4,
                    Number=205,
                    Type=Enums.RoomType.DoubleBed
                }
           });

            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(RoomsSeeder)}");
        }
    }
}
