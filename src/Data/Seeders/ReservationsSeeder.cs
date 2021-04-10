using Data.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Model layer related to automated data population on database creation
/// </summary>
namespace Data.Seeders
{
    /// <summary>
    /// Reservations table seeder
    /// </summary>
    public class ReservationsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, ILogger logger)
        {
            if(dbContext.Reservations.Any())
            {
                return;
            }

            await dbContext.Reservations.AddRangeAsync(new Reservation[]
           {
                new Reservation
                {
                    Id="ExampleReservation1",
                    AccommodationDate=DateTime.Today.AddDays(2),
                    ReleaseDate=DateTime.Today.AddDays(5),
                    AllInclusive=true,
                    Breakfast=false,
                    Price=205,
                    Room=dbContext.Rooms.FirstOrDefault(x=>x.Id=="ExampleRoom1"),
                    User=dbContext.Users.FirstOrDefault(x=>x.Id=="Admin"),
                },
                new Reservation
                {
                    Id="ExampleReservation2",
                    AccommodationDate=DateTime.Today.AddDays(8),
                    ReleaseDate=DateTime.Today.AddDays(10),
                    AllInclusive=false,
                    Breakfast=true,
                    Price=155,
                    Room=dbContext.Rooms.FirstOrDefault(x=>x.Id=="ExampleRoom2"),
                    User=dbContext.Users.FirstOrDefault(x=>x.Id=="Admin"),
                },
           }) ;

            await dbContext.SaveChangesAsync();
            logger.LogInformation($"Finished executing {nameof(ReservationsSeeder)}");
        }
    }
}
