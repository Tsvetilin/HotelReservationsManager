using Data;
using Data.Models;
using Microsoft.AspNetCore.Identity;
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
using Web.Models.Reservations;

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
                                                                .SeedAsync(settings)
                                                                .SeedAsync(users)
                                                                .SeedAsync(rooms)
                                                                .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.AddReservation(Reservations.Reservation1User3Room1NoClient.Room.Id,
                                         Reservations.Reservation1User3Room1NoClient.AccommodationDate,
                                         Reservations.Reservation1User3Room1NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation1User3Room1NoClient.Clients,
                                         Reservations.Reservation1User3Room1NoClient.User
                                         );

            // Assert
            Assert.NotNull(reservation);
            Assert.AreEqual(1, context.Reservations.Count());
        }

        [Test]
        public async Task AddReservation_ShouldNotAddReservation()
        {
            // Arange
            List<Reservation> reservationsData = new() { Reservations.Reservation3User4Room2NoClient };
            List<Setting> settings = new() { Settings.AllInclusive, Settings.Breakfast };
            List<Room> rooms = new() { Rooms.Room2 };
            List<ApplicationUser> users = new() { Users.User4NotEmployee };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(settings)
                                                                .SeedAsync(users)
                                                                .SeedAsync(rooms)
                                                                .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.AddReservation(Reservations.Reservation4User4Room2NoClient.Room.Id,
                                         Reservations.Reservation4User4Room2NoClient.AccommodationDate,
                                         Reservations.Reservation4User4Room2NoClient.ReleaseDate,
                                         Reservations.AllInClusive1,
                                         Reservations.Breakfast1,
                                         Reservations.Reservation4User4Room2NoClient.Clients,
                                         Reservations.Reservation4User4Room2NoClient.User
                                         );

            // Assert
            Assert.Null(reservation);
        }

        [Test]
        public async Task UpdateReservation_ShouldUpdateReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient
            };

            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Room> rooms = new()
            {
                Rooms.Room1
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            List<ClientData> clients = new()
            {
                Users.Client1User
            };

            List<ApplicationRole> applicationRoles = new()
            {
                new ApplicationRole { Name = "User" }
            };

            List<IdentityUserRole<string>> identityUserRoles = new()
            {
                new IdentityUserRole<string>
                {
                    RoleId = applicationRoles.First().Id,
                    UserId = Users.User3NotEmployee.Id
                }
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(settings)
                                                                    .SeedAsync(clients)
                                                                    .SeedAsync(applicationRoles)
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(identityUserRoles)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData);
            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var result = await service.UpdateReservation(Reservations.Reservation1User3Room1NoClient.Id,
                                            DateTime.Today.AddDays(7),
                                            DateTime.Today.AddDays(9),
                                            Reservations.UpdateAllInClusive1,
                                            Reservations.UpdateBreakfast1,
                                            clients,
                                            Reservations.Reservation1User3Room1NoClient.User
                                         );

            // Assert
            Assert.AreEqual(true, result);
        }

        [Test]
        public async Task DeleteReservation_ShouldDeleteReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient
            };

            List<Room> rooms = new()
            {
                Rooms.Room1
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            bool result1 = await service.DeleteReservation(Reservations.Reservation1User3Room1NoClient.Id);
            bool result2 = await service.DeleteReservation("2");

            // Assert
            Assert.IsTrue(result1);
            Assert.IsFalse(result2);
        }

        [Test]
        public async Task GetReservation_ShouldReturnReservation()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient,
                Reservations.Reservation2User4Room2NoClient
            };

            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee,
                Users.User4NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var reservation = await service.GetReservation<ReservationViewModel>(Reservations.Reservation2User4Room2NoClient.Id);

            // Assert
            Assert.AreEqual(Reservations.Reservation2User4Room2NoClient.Id, reservation.Id);
            Assert.AreEqual(Reservations.Reservation2User4Room2NoClient.ReleaseDate, reservation.ReleaseDate);
            Assert.AreEqual(Reservations.Reservation2User4Room2NoClient.Price, reservation.Price);
        }

        [Test]
        public async Task GetReservationsForUser_ReturnsUsersReservations()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient,
                Reservations.Reservation2User4Room2NoClient,
                Reservations.Reservation3User4Room2NoClient
            };

            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee,
                Users.User4NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(settings)
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData)
                                                                    ;

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var userReservations = await service.GetReservationsForUser<ReservationViewModel>(Users.User4NotEmployee.Id);

            // Assert
            Assert.AreEqual(2, userReservations.Count());
        }

        [Test]
        public async Task GetForUserOnPage_ShouldReturnReservationsForUserOnPage()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient,
                Reservations.Reservation2User4Room2NoClient,
                Reservations.Reservation3User4Room2NoClient
            };

            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee,
                Users.User4NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(settings)
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var userReservations = await service.GetForUserOnPage<ReservationViewModel>(Users.User4NotEmployee.Id, 1, 2);

            // Assert
            Assert.AreEqual(2, userReservations.Count());
        }

        [Test]
        public async Task GetAll_ShouldReturnAllReservations()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient,
                Reservations.Reservation2User4Room2NoClient,
                Reservations.Reservation3User4Room2NoClient
            };

            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee,
                Users.User4NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(settings)
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var allReservations = await service.GetAll<ReservationViewModel>();

            // Assert
            Assert.AreEqual(3, allReservations.Count());
        }

        [Test]
        public async Task CountAll_ShouldReturnTheCountOfAllReservations()
        {
            // Arange
            List<Reservation> reservationsData = new()
            {
                Reservations.Reservation1User3Room1NoClient,
                Reservations.Reservation2User4Room2NoClient,
                Reservations.Reservation3User4Room2NoClient
            };

            List<Setting> settings = new()
            {
                Settings.AllInclusive,
                Settings.Breakfast
            };

            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            List<ApplicationUser> users = new()
            {
                Users.User3NotEmployee,
                Users.User4NotEmployee
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                    .SeedAsync(settings)
                                                                    .SeedAsync(users)
                                                                    .SeedAsync(rooms)
                                                                    .SeedAsync(reservationsData);

            SettingService settingService = new(context);

            var service = new ReservationsService(context, settingService);

            // Act
            var allReservationsCount = await service.CountAllReservations();

            // Assert
            Assert.AreEqual(3, allReservationsCount);
        }
    }
}
