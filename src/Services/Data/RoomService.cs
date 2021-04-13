using Data;
using Data.Enums;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Services.Data
{
    public class RoomServices : IRoomService
    {
        private readonly ApplicationDbContext context;

        public RoomServices(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Adds room to the database
        /// </summary>
        /// <param name="room">New room object</param>
        /// <returns>Task representing the operation</returns>
        public async Task AddRoom(Room room)
        {
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds all rooms that have the searched capacity
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <param name="capacity">The searched room capacity</param>
        /// <returns>Task with the rooms data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllByCapacity<T>(int capacity)
        {
            return await context.Rooms.Where(x => x.Capacity == capacity).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all rooms that have the searched type
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <param name="type">The searched room type</param>
        /// <returns>Task with the rooms data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllByType<T>(RoomType type)
        {
            return await context.Rooms.Where(x => x.Type == type).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all rooms that are available at present
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <returns>Task with the rooms data that satisfy the criteria parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAllFreeRoomsAtPresent<T>()
        {
            return await context.Rooms.
                                 Where(x => !x.Reservations.Any(r => r.AccommodationDate <= DateTime.Today &&
                                                                     r.ReleaseDate > DateTime.Today)).
                                 OrderBy(x => x.Number).
                                 ProjectTo<T>().
                                 ToListAsync();
        }

        /// <summary>
        /// Finds all rooms that are available at present
        /// </summary>
        /// <returns>Task with the count of the rooms data that satisfy the criteria parsed to 
        /// <typeparamref name="T"/> object or null if not found</returns>
        public async Task<int> CountFreeRoomsAtPresent()
        {
            return await context.Rooms.
                                 Where(x => !x.Reservations.Any(r => r.AccommodationDate <= DateTime.Today 
                                                                  && r.ReleaseDate > DateTime.Today)).
                                 CountAsync();
        }

        /// <summary>
        /// Finds all rooms in the database
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <returns>Task with all rooms data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await context.Rooms.AsQueryable().ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds all rooms that satisfy the criteria
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <param name="availableOnly">Indicate if the rooms should be available</param>
        /// <param name="types">The filtered rooms type<sparam>
        /// <param name="minCapacity">The min capacity of the filtered rooms</param>
        /// <returns>Task with filtered rooms data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetSearchResults<T>(bool availableOnly = false, 
                                                              RoomType[] types = null, 
                                                              int? minCapacity = null)
        {
            IQueryable<Room> result = context.Rooms;

            if (availableOnly)
            {
                result = result.Where(x => !x.Reservations.Any(r => r.AccommodationDate <= DateTime.Today
                                                                 && r.ReleaseDate > DateTime.Today));
            }

            if (types != null && (types?.Count() ?? 0) > 0)
            {
                result = result.Where(x => types.Contains(x.Type));
            }

            if (minCapacity != null && minCapacity > 0)
            {
                result = result.Where(x => x.Capacity > minCapacity);
            }

            return await result.OrderBy(x => x.Number).ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Removes room from the database
        /// </summary>
        /// <param name="id">The room to delete id</param>
        /// <returns>Task representing the operation</returns>
        public async Task DeleteRoom(string id)
        {
            var room = await context.Rooms.Include(x => x.Reservations).FirstOrDefaultAsync(x => x.Id == id);
            if (room != null)
            {
                //Feature: Send an email for room cancel forced
                if (room.Reservations != null)
                {
                    context.Reservations.RemoveRange(room.Reservations);
                    await context.SaveChangesAsync();
                }

                context.Rooms.Remove(room);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Updates the data of existing room
        /// </summary>
        /// <param name="id">Existing room id</param>
        /// <param name="room">Room data to change with</param>
        /// <returns>Task representing the operation</returns>
        public async Task UpdateRoom(string id, Room room)
        {
            room.Id = id;
            var roomToChange = await context.Rooms.AsNoTracking().Include(x=>x.Reservations).FirstOrDefaultAsync(x=>x.Id==id);
            if (roomToChange != null)
            {
                if (roomToChange.Reservations != null)
                {
                    foreach (var reservation in roomToChange.Reservations)
                    {
                        if (roomToChange.Capacity < room.Capacity)
                        {
                            //Feature: Send an email for change & not cancel till confirmation
                            context.Reservations.Remove(reservation);
                        }
                    }
                }

                context.Rooms.Update(room);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Finds the searched room
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <param name="id">Searched room id</param>
        /// <returns>Task with the room data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<T> GetRoom<T>(string id)
        {
            return await this.context.Rooms.Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        ///<returns>The count of all rooms in the database</returns>
        public int CountAllRooms()
        {
            return context.Rooms.Count();
        }

        /// <summary>
        /// Finds the lowest adult price of the rooms in the database
        /// </summary>
        /// <returns>Task with the minimum price result</returns>
        public async Task<double> GetMinPrice()
        {
            return await this.context.Rooms.OrderBy(x => x.AdultPrice).Select(X => X.AdultPrice).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds the highest adult price of the rooms in the database
        /// </summary>
        /// <returns>Task with the maximum price result</returns>
        public async Task<double> GetMaxPrice()
        {
            return await this.context.Rooms.
                                      OrderByDescending(x => x.AdultPrice).
                                      Select(X => X.AdultPrice).
                                      FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds if a room is free
        /// </summary>
        /// <param name="number">The searched room number</param>
        /// <param name="roomId">The room numer to update, to exclude its Number from the search</param>
        /// <returns>Task with the room numer availability result</returns>
        public async Task<bool> IsRoomNumberFree(int number, string roomId = null)
        {
            return !await context.Rooms.AsNoTracking().Where(x => x.Id != roomId).AnyAsync(x => x.Number == number);
        }

        /// <summary>
        /// Finds the highest capacity of the rooms in the database
        /// </summary>
        /// <returns>Task with the maximum room capacity result</returns>
        public async Task<int> GetMaxCapacity()
        {
            return await context.Rooms.AsNoTracking().
                                       OrderByDescending(x => x.Capacity).
                                       Select(x => x.Capacity).
                                       FirstOrDefaultAsync();
        }
    }
}
