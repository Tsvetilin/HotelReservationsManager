using Data;
using Data.Enums;
using Data.Models;
using NUnit.Framework;
using Services;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Common;
using Tests.Data;
using Web.Models;
using Web.Models.Rooms;

namespace Tests.Service.Tests
{
    public class RoomServiceTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            MappingConfig.RegisterMappings(typeof(ErrorViewModel).Assembly);
        }

        [Test]
        public async Task AddRoom_ShouldAddARoom()
        {
            //Arange
            List<Room> rooms = new() { Rooms.Room1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            await roomService.AddRoom(Rooms.Room2);

            //Assert
            Assert.AreEqual(rooms.Count + 1, context.Rooms.Count());
        }

        [Test]
        public async Task GetRoom_ShouldGetARoomById()
        {
            //Arrange
            List<Room> rooms = new() { Rooms.Room1 };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);

            var roomService = new RoomServices(context);

            //Act
            var resultRoom = await roomService.GetRoom<RoomViewModel>(Rooms.Room1.Id);

            //Assert
            Assert.IsNotNull(resultRoom);
            Assert.AreEqual(Rooms.Room1.Id, resultRoom.Id);
            Assert.AreEqual(Rooms.Room1.Capacity, resultRoom.Capacity);
        }
        [Test]
        public async Task GetAll_ShouldGetAllRooms()
        {
            //Arrange
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            var resultList = await roomService.GetAll<RoomViewModel>();

            //
            Assert.IsNotNull(resultList);
            Assert.AreEqual(rooms.Count, resultList.Count());
        }
        [Test]
        public async Task Update_ShouldUpdateARoom()
        {
            //Arrange
            List<Room> rooms = new()
            {
                Rooms.Room1
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            await roomService.UpdateRoom(Rooms.Room1.Id, Rooms.Room2);

            //Assert      
            Assert.AreEqual(context.Rooms.FirstOrDefault().IsTaken, Rooms.Room2.IsTaken);
            Assert.AreEqual(context.Rooms.FirstOrDefault().Number, Rooms.Room2.Number);
            Assert.AreEqual(context.Rooms.FirstOrDefault().AdultPrice, Rooms.Room2.AdultPrice);
            Assert.AreEqual(context.Rooms.FirstOrDefault().ChildrenPrice, Rooms.Room2.ChildrenPrice);
            Assert.AreEqual(context.Rooms.FirstOrDefault().Capacity, Rooms.Room2.Capacity);
            Assert.AreEqual(context.Rooms.FirstOrDefault().Type, Rooms.Room2.Type);
        }

        [Test]
        public async Task DeleteRoom_ShouldRemoveARoom()
        {
            //Arrange
            List<Room> rooms = new()
            {
                Rooms.Room1
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            await roomService.DeleteRoom(Rooms.Room1.Id);

            //Assert
            Assert.AreEqual(0, context.Rooms.Count());
        }

        [Test]
        public async Task CountAllFreeRoomsAtPresent_ShouldCountAllFreeRoomsAtPresent()
        {
            //Arrange
            List<Room> rooms = new()
            {
                Rooms.Room1TakenAtPresent,
                Rooms.Room2TakenAtPresent,
                Rooms.Room1FreeAtPresent
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            var count = await roomService.CountFreeRoomsAtPresent();

            //Assert
            Assert.AreEqual(1, count);
        }
        [Test]
        public async Task GetAllFreeAtPresentRooms_ShouldReturnAllFreeAtPresentRooms()
        {
            //Arrange
            List<Room> rooms = new()
            {
                Rooms.Room1TakenAtPresent,
                Rooms.Room2TakenAtPresent,
                Rooms.Room1FreeAtPresent
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            var result = await roomService.GetAllFreeRoomsAtPresent<RoomViewModel>();

            //Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public async Task CountAllRooms_ShouldCountAllRooms()
        {
            //Arrange
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);
            //Act
            var count = await roomService.CountFreeRoomsAtPresent();
            //Assert
            Assert.AreEqual(2, count);
        }

        [Test]
        public async Task GetSearchResults_ShouldReturnAllRoomsContainingTheCriteria()
        {
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);

            //Act
            var result1 = await roomService.GetSearchResults<RoomViewModel>(RoomType.Apartment.ToString());
            var result2 = await roomService.GetSearchResults<RoomViewModel>("4");

            //Assert
            Assert.NotNull(result1);
            Assert.NotNull(result2);
            Assert.AreEqual(Rooms.Room1.Id, result1.FirstOrDefault().Id);
            Assert.AreEqual(Rooms.Room1.Number, result1.FirstOrDefault().Number);
            Assert.AreEqual(Rooms.Room1.Type, result1.FirstOrDefault().Type);
            Assert.AreEqual(Rooms.Room2.Type, result2.FirstOrDefault().Type);
            Assert.AreEqual(Rooms.Room2.Type, result2.FirstOrDefault().Type);
            Assert.AreEqual(Rooms.Room2.Type, result2.FirstOrDefault().Type);
        }

        [Test]
        public async Task GetAllByCapacity_ShouldGetAllRoomsByCapacity()
        {
            //Arange
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);
            //Act
            var result = await roomService.GetAllByCapacity<RoomViewModel>(4);
            //Arrange
            Assert.AreEqual(1, result.Count());
        }
        [Test]
        public async Task GetAllByType_ShouldGetAllRoomsByType()
        {
            //Arange
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };

            ApplicationDbContext context = await InMemoryFactory.InitializeContext()
                                                                .SeedAsync(rooms);
            var roomService = new RoomServices(context);
            
            //Act
            var result1 = await roomService.GetAllByType<RoomViewModel>(Rooms.Room2.Type);
            var result2 = await roomService.GetAllByType<RoomViewModel>(Rooms.Room1.Type);

            //Arrange
            Assert.AreEqual(1, result1.Count());
            Assert.AreEqual(1, result2.Count());
        }
    }
}
