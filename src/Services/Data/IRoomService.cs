using System;
using Data.Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Data.Enums;

namespace Services
{
    public interface IRoomService
    {
        public Task AddRoom(Room room);

        public Task<IEnumerable<T>> GetAllByCapacity<T>(int capacity);

        public Task<IEnumerable<T>> GetAllByType<T>(RoomType type);

        public Task<IEnumerable<T>> GetAllReservedRooms<T>();

        public Task DeleteRoom(string id);

        public Task UpdateRoom(Room room);

        public Task<T> GetRoom<T>(string id);
       
    }
  
}
