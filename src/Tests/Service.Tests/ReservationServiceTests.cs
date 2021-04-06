using Data;
using Data.Models;
using NUnit.Framework;
using Services;
using Services.Data;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data;
using Web.Models;

namespace Tests.Service.Tests
{
    class ReservationServiceTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public async Task AddReservation_ShouldAddReservation()
        {
            // Arange
            List<Reservation> reservationsData = new() { };
            List<Setting> settings = new() { Settings.AllInclusive, Settings.Breakfast };
            List<Room> rooms = new() { Rooms.Room1 };
            List<ApplicationUser> users = new() { Users.User3NotEmployee };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(reservationsData)
                                                                .SeedAsync(settings)
                                                                .SeedAsync(rooms)
                                                                .SeedAsync(users);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            await service.AddReservation(Reservations.Reservation1User3Room1NoClient.Room.Id,
                                         Reservations.Reservation1User3Room1NoClient.AccommodationDate,
                                         Reservations.Reservation1User3Room1NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation1User3Room1NoClient.Clients,
                                         Reservations.Reservation1User3Room1NoClient.User
                                         );


            // Assert
            Assert.AreEqual(reservationsData.Count() + 1, context.Reservations.Count());
        }

        [Test]
        public async Task UpdateReservation_ShouldUpdateReservation()
        {
            // Arange
            List<Reservation> reservationsData = new() { Reservations.Reservation1User3Room1NoClient };
            List<Setting> settings = new() { Settings.AllInclusive, Settings.Breakfast };
            List<Room> rooms = new() { Rooms.Room1 };
            List<ApplicationUser> users = new() { Users.User3NotEmployee };
            List<ClientData> clients = new() { Users.Client1User };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(settings)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(clients)
                                                                    .SeedAsync(reservationsData)
                                                                    ;

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            await service.UpdateReservation(Reservations.Reservation1User3Room1NoClient.Id,
                                            DateTime.Today.AddDays(7),
                                            DateTime.Today.AddDays(9),
                                            Reservations.UpdateAllInClusive1,
                                            Reservations.UpdateBreakfast1,
                                            clients,
                                            Reservations.Reservation1User3Room1NoClient.User
                                         );

            // Assert
            Assert.AreEqual(reservationsData.Count(), context.Reservations.Count());
            
            Assert.AreEqual(reservationsData.FirstOrDefault().Id,
                               context.Reservations.FirstOrDefault().Id);
            Assert.AreEqual(reservationsData.FirstOrDefault().Room.Id,
                               context.Reservations.FirstOrDefault().Room.Id);
            Assert.AreEqual(reservationsData.FirstOrDefault().User.Id,
                               context.Reservations.FirstOrDefault().User.Id);
            Assert.AreNotEqual(reservationsData.FirstOrDefault().Clients.Count(),
                               context.Reservations.FirstOrDefault().Clients.Count());
            Assert.AreNotEqual(reservationsData.FirstOrDefault().Breakfast,
                               context.Reservations.FirstOrDefault().Breakfast);
            Assert.AreNotEqual(reservationsData.FirstOrDefault().AllInclusive,
                               context.Reservations.FirstOrDefault().AllInclusive); 
            Assert.Greater(0, context.Reservations.FirstOrDefault().Price);
            Assert.GreaterOrEqual(DateTime.Today, context.Reservations.FirstOrDefault().AccommodationDate);
            Assert.GreaterOrEqual(context.Reservations.FirstOrDefault().AccommodationDate,
                                    context.Reservations.FirstOrDefault().ReleaseDate);
        }
        /*
        public async Task DeleteReservation(string id)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                this.dbContext.ClientData.RemoveRange(this.dbContext.ClientData.Where(x => x.Reservation.Id == reservation.Id));
                this.dbContext.Reservations.Remove(reservation);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<T> GetReservation<T>(string id)
        {
            return await this.dbContext.Reservations.Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetReservationsForUser<T>(string userId)
        {
            return await this.dbContext.Reservations.Where(x => x.User.Id == userId).OrderByDescending(x => x.AccommodationDate).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetForUserOnPage<T>(string userId, int page, int elementsOnPage)
        {
            return await GetReservationsForUser<T>(userId).GetPageItems(page, elementsOnPage);
        }

        public async Task<IEnumerable<ClientData>> UpdateClientsForReservation(string reservationId, IEnumerable<ClientData> clients)
        {
            var reservation = await dbContext.Reservations.FindAsync(reservationId);
            var initialClients = await dbContext.ClientData.Where(x => x.Reservation.Id == reservationId).ToListAsync();

            if (initialClients.Count() > clients.Count())
            {
                var deletedClients = initialClients.Where(x => !clients.Select(u => u.Id).Contains(x.Id)).ToList();
                dbContext.ClientData.RemoveRange(deletedClients);
            }

            var newClients = clients.Where(x => !initialClients.Select(u => u.Id).Contains(x.Id) && x.Id != null).ToList();
            if (newClients?.Any() ?? false)
            {
                dbContext.ClientData.AddRange(newClients);
            }

            var clientsToUpdate = clients.Where(x => !newClients.Select(u => u.Id).Contains(x.Id) && x.Id != null);
            if (clientsToUpdate?.Any() ?? false)
            {
                dbContext.ClientData.UpdateRange(clientsToUpdate);
            }

            await dbContext.SaveChangesAsync();

            return clients;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.dbContext.Reservations.OrderBy(x => x.ReleaseDate).ProjectTo<T>().ToListAsync();
        }
*/
    }
}
