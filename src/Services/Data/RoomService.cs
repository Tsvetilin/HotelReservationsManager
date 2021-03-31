using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Mapping;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
     public class RoomServices : IRoomService
    {
        private readonly ApplicationDbContext context;
        public RoomServices(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task AddRoom(Room room)
        {
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetAllByCapacity<T>(int capacity)
        {
            return await context.Rooms.Where(x => x.Capacity == capacity).ProjectTo<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllByType<T>(RoomType type)
        {
            return await context.Rooms.Where(x => x.Type == type).ProjectTo<T>().ToListAsync();
        }
        public async Task<IEnumerable<T>> GetAllReservedRooms<T>()
        {
            return await context.Rooms.Where(x => x.IsTaken).ProjectTo<T>().ToListAsync();
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
        public async Task<T> GetRoom<T>(string id)
        {
            return await this.context.Reservations.Where(x=>x.Id==id).ProjectTo<T>().FirstOrDefaultAsync();
        }
    }
}
