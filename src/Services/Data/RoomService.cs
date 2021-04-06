using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Common;
using Services.External;

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

        public async Task<IEnumerable<T>> GetAllFreeRoomsAtPresent<T>()
        {
            return await context.Rooms.
                Where(x => !x.Reservations.Any(r=>r.AccommodationDate<DateTime.Today && r.ReleaseDate>DateTime.Today)).
                OrderBy(x=>x.Number).
                ProjectTo<T>().
                ToListAsync();
        }

        public async Task<int> CountFreeRoomsAtPresent()
        {
            return await context.Rooms.
                Where(x => !x.Reservations.Any(r => r.AccommodationDate < DateTime.Today && r.ReleaseDate > DateTime.Today)).
                CountAsync();
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await context.Rooms.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPageItems<T>(int page, int roomsOnPage)
        {
            return await GetAll<T>().GetPageItems(page,roomsOnPage);
        }

        public async Task<IEnumerable<T>> GetSearchResults<T>(string searchString)
        {
            var result = new List<T>();
            var capacityResults = await GetAllByCapacity<T>(int.Parse(searchString));
            var typeResults = await GetAllByType<T>(((RoomType)Enum.Parse(typeof(RoomType), searchString)));
           
            if (capacityResults != null)
            {
                result.AddRange(capacityResults);
            }
            if (typeResults != null)
            {
                result.AddRange(typeResults);
            }
            return result;
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

        // TODO: Cancel reservations if less Capacity than it was before & Send Email
        public async Task UpdateRoom(string id,Room room)
        {
            var roomToChange = await context.Rooms.FindAsync(id);
            if (roomToChange != null)
            {
                if(roomToChange.Reservations!=null)
                {
                 foreach(var reservation in roomToChange.Reservations)
                    {
                        if(roomToChange.Capacity <room.Capacity)
                        {
                            //TODO: Send an email for cancelation
                            context.Reservations.Remove(reservation);
                        }
                    }
                }
                context.Entry(roomToChange).CurrentValues.SetValues(room);
                await context.SaveChangesAsync();
            }
        }
        public async Task<T> GetRoom<T>(string id)
        {
            return await this.context.Rooms.Where(x=>x.Id==id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public  int CountAllRooms()
        {
            return context.Rooms.Count();
        }

        public async Task<double> GetMinPrice()
        {
            return await this.context.Rooms.OrderBy(x => x.AdultPrice).Select(X => X.AdultPrice).FirstOrDefaultAsync();
        }

        public async Task<double> GetMaxPrice()
        {
            return await this.context.Rooms.OrderByDescending(x => x.AdultPrice).Select(X => X.AdultPrice).FirstOrDefaultAsync();
        }
    }
}
