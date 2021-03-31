using AutoMapper;
using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
     public class RoomServices : IRoomService
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        public RoomServices(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddRoom(Room room)
        {
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllByCapacity<T>(int capacity) where T:class
        {
            var roomsInContext = await context.Rooms.Where(x => x.Capacity == capacity).ToListAsync();
            return mapper.Map<List<Room>, IEnumerable<T>>(roomsInContext);
        }
        public async Task<IEnumerable<T>> GetAllByType<T>(RoomType type) where T:class
        {
            var roomsInContext = await context.Rooms.Where(x => x.Type == type).ToListAsync();
            return mapper.Map<List<Room>, IEnumerable<T>>(roomsInContext);
        }
        public async Task<IEnumerable<T>> GetAllReservedRooms<T>() where T:class
        {
            var reservedRooms = await context.Rooms.Where(x => x.IsTaken == true).ToListAsync();
            return mapper.Map<List<Room>, IEnumerable<T>>(reservedRooms);
        }
        public async Task DeleteRoom(string id)
        {
            var room = await context.Rooms.FindAsync(id);
            if(room !=null)
            {
                context.Rooms.Remove(room);
                await context.SaveChangesAsync();
            }
        }
        public async Task UpdateRoom(Room room)
        {
            var roomToChange = await context.Rooms.FindAsync(room.Id);
            if (roomToChange != null)
            {
                context.Entry(roomToChange).CurrentValues.SetValues(room);
                await context.SaveChangesAsync();
            }
        }
        public async Task<T> GetRoom<T>(string id) where T : class
        {
            var room = await this.context.Reservations.FindAsync(id);
            return mapper.Map(room, typeof(Reservation), typeof(T)) as T;
        }
    }
}
