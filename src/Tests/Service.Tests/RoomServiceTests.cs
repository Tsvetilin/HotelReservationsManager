using Data;
using Data.Models;
using NUnit.Framework;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Service.Tests
{
    public class RoomServiceTests
    {
        [Test]
        public async Task GetUser_ShouldGetARoom()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                new Room(),
                new Room()
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
                new Room(),
                new Room(),
                new Room()
            };
            context.Add(rooms);
            var roomService = new RoomServices(context);

            //Act
            var resultList = await roomService.GetAll<Room>();

            //
            Assert.IsNotNull(resultList);
            Assert.AreEqual(rooms.Count, resultList.Count(), "Not equal");
        }
        [Test]
        public async Task Update_ShouldUpdateARoom()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                new Room(),
            };
            context.Add(rooms);
            Room room = new();
            var roomService = new RoomServices(context);
            //Act
            await roomService.UpdateRoom(rooms.First().Id, room);
            //Assert
            Assert.AreEqual(room, rooms.First());
        }
        [Test]
        public async Task DeleteRoom_ShouldRemoveARoom()
        {
            //Arrange
            var context = new ApplicationDbContext();
            List<Room> rooms = new()
            {
                new Room(),
            };
            context.Add(rooms);
            var roomToRemove = rooms.First();
            var roomService = new RoomServices(context);
            //Act
            await roomService.DeleteRoom(roomToRemove.Id);
            //Assert
            Assert.AreEqual(roomService.CountAllRooms(), 0);
        }


    }
}
