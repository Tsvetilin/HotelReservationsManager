using Data.Models;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Seeders
{
    /// <summary>
    /// Rooms table seeder
    /// </summary>
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
                    AdultPrice=50,
                    ChildrenPrice=30,
                    Capacity=5,
                    Number=105,
                    Type=Enums.RoomType.Apartment,
                    ImageUrl="https://cf.bstatic.com/images/hotel/max1024x768/197/197179243.jpg",
                },
                new Room
                {
                    Id="ExampleRoom2",
                    AdultPrice=30,
                    ChildrenPrice=10,
                    Capacity=3,
                    Number=205,
                    Type=Enums.RoomType.DoubleBed,
                    ImageUrl="https://pix10.agoda.net/hotelImages/5668227/0/7542736b26b0676a0e9e3c4aab831241.jpg?s=1024x768",
                }
           });

            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(RoomsSeeder)}");
        }
    }
}
