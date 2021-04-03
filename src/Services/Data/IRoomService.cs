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
        public Task<IEnumerable<T>> GetAll<T>();
        public Task<IEnumerable<T>> GetSearchResults<T>(string searchString);

        public Task DeleteRoom(string id);

        public Task UpdateRoom(string id,Room room);

        public Task<T> GetRoom<T>(string id);
        public Task<IEnumerable<T>> GetPageItems<T>(int page, int roomsOnPage);
        public int CountAllRooms();


    }
  
}
