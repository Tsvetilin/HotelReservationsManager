using Data;
using Data.Models;
using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Data;

namespace Tests.Service.Tests
{
    public class RoomServiceTests
    {
        
        [Test]
        public async Task GetRoom_ShouldGetARoomById()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            var searchedRoom = rooms.First();

            //Act
            var resultRoom = await roomService.GetRoom<Room>(searchedRoom.Id);

            //Assert
            Assert.IsNotNull(resultRoom);
            Assert.AreEqual(searchedRoom.Id, resultRoom.Id, "Not equal");
        }
        [Test]
        public async Task GetAll_ShouldGetAllRooms()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);

            //Act
            var resultList = await roomService.GetAll<Room>();

            //
            Assert.IsNotNull(resultList);
            Assert.AreEqual(rooms.Count, resultList.Count());
        }
        [Test]
        public async Task Update_ShouldUpdateARoom()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1
            };
            context.Add(rooms);
            Room room = Rooms.Room2;
            var roomService = new RoomServices(context);
            //Act
            await roomService.UpdateRoom(rooms.First().Id, room);
            //Assert      
            Assert.AreSame(room, rooms.First());
        }
        [Test]
        public async Task DeleteRoom_ShouldRemoveARoom()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1
            };
            context.Add(rooms);
            var roomToRemove = rooms.First();
            var roomService = new RoomServices(context);
            //Act
            await roomService.DeleteRoom(roomToRemove.Id);
            //Assert
            Assert.AreEqual(roomService.CountAllRooms(), 0);
        }
        public async Task CountAllFreeRooms()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            var count = await roomService.CountFreeRoomsAtPresent();
            //Assert
            Assert.AreEqual(count, 1);
        }
        [Test]
        public async Task CountAllRooms_ShouldReturnAllFreeRooms()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            var result = await roomService.GetAllFreeRoomsAtPresent<Room>();
            //Assert
            Assert.AreEqual(result.Count(), 1);
        }
        [Test]
        public void CountAllRooms_ShouldCountAllRooms()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            var count = roomService.CountAllRooms();
            //Assert
            Assert.AreEqual(count, 2);
        }
        [Test]
        public async Task Add_ShouldAddARoom()
        {
            //Arange
                var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            await roomService.AddRoom(Rooms.Room2);
            //Assert
            Assert.AreEqual(roomService.CountAllRooms(),2);
        }
        [Test]
        public async Task GetAllByCriteria_ShouldReturnAllRoomsContainingTheCriteria()
        {
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            var result1 = await roomService.GetSearchResults<Room>("Apartment");
            var result2 = await roomService.GetSearchResults<Room>("4");
            //Assert
            Assert.AreSame(result1, rooms.First());
            Assert.AreSame(result2, rooms[1]);
        }
        public async Task Get_ShouldGetAllRoomsByCapacity()
        {
            //Arange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            var result = await roomService.GetAllByCapacity<Room>(4);
            //Arrange
            Assert.AreEqual(result.Count(), 1);
        }
        [Test]
        public async Task Get_ShouldGetAllRoomsByType()
        {
            //Arange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                Rooms.Room1,
                Rooms.Room2
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);
            //Act
            var result = await roomService.GetAllByType<Room>(Rooms.Room2.Type);
            //Arrange
            Assert.AreEqual(result.Count(), 1);
        }
    }
}
