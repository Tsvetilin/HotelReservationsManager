using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Data
{
    public class ReservationsService : IReservationService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly ISettingService settingService;

        public ReservationsService(ApplicationDbContext dbContext, ISettingService settingService)
        {
            this.dbContext = dbContext;
            this.settingService = settingService;
        }

        /// <summary>
        /// Checks if the dates for a reservation are valid and if the room is free in that period
        /// </summary>
        /// <param name="roomId">Room's id</param>
        /// <param name="accomodationDate">Reservation's accomodation date</param>
        /// <param name="releaseDate">Reservation's release date</param>
        /// <param name="reservationId">Reservations to update id or null if making new reservation</param>
        /// <returns>Task with room's dates for reservatioin validity result</returns>
        public async Task<bool> AreDatesAcceptable(string roomId,
                                                    DateTime accomodationDate,
                                                    DateTime releaseDate,
                                                    string reservationId = null)
        {
            if (accomodationDate >= releaseDate || accomodationDate < DateTime.Today)
            {
                return false;
            }

            var reservationPeriods = await dbContext.
                                           Reservations.
                                           AsNoTracking().
                                           Where(x => x.Room.Id == roomId).
                                           Select(x => new Tuple<DateTime, DateTime>
                                                        (x.AccommodationDate, x.ReleaseDate).
                                                        ToValueTuple()).
                                          ToListAsync();

            if (!string.IsNullOrWhiteSpace(reservationId))
            {
                var reservation = await dbContext.Reservations.AsNoTracking().FirstOrDefaultAsync(x => x.Id == reservationId);
                reservationPeriods = reservationPeriods.Where(x => x.Item1 != reservation.AccommodationDate &&
                                                              x.Item2 != reservation.ReleaseDate).ToList();
            }

            return !reservationPeriods.Any(x =>
                (x.Item1 >= accomodationDate && x.Item1 <= releaseDate) ||
                (x.Item2 > accomodationDate && x.Item2 <= releaseDate) ||
                (x.Item1 >= accomodationDate && x.Item2 <= releaseDate) ||
                (x.Item1 <= accomodationDate && x.Item2 >= releaseDate));
        }

        /// <summary>
        /// Calculates the reservation total price
        /// </summary>
        /// <param name="room">The reservation room</param>
        /// <param name="clients">The room clients</param>
        /// <param name="allInclusive">Reservation's order all inclusive</param>
        /// <param name="breakfast">Reservation's order breakfast</param>
        /// <returns>Task with the calculation result</returns>
        private async Task<double> CalculatePriceForNight(Room room,
                                                  IEnumerable<ClientData> clients,
                                                  bool allInclusive,
                                                  bool breakfast)
        {
            clients = clients.ToList().Where(x => x.FullName != null);
            var price =
                clients.Count(x => x.IsAdult) * room.AdultPrice +
                clients.Count(x => !x.IsAdult) * room.ChildrenPrice +
                room.AdultPrice;

            if (allInclusive)
            {
                price += double.Parse((await settingService
                                                .GetAsync($"{nameof(Reservation.AllInclusive)}Price")).Value);
            }
            else if (breakfast)
            {
                price += double.Parse((await settingService
                                               .GetAsync($"{nameof(Reservation.Breakfast)}Price")).Value);
            }

            return price;
        }

        /// <summary>
        /// Add reservation to database
        /// </summary>
        /// <param name="roomId">The room id</param>
        /// <param name="accomodationDate">The reservation accomodation date</param>
        /// <param name="releaseDate">The reservation release date </param>
        /// <param name="allInclusive">Reservation's order all inclusive</param>
        /// <param name="breakfast">Reservation's order breakfast</param>
        /// <param name="clients">The room's clients</param>
        /// <param name="user">The room renter</param>
        /// <returns>Task with the new reservation result</returns>
        public async Task<Reservation> AddReservation(string roomId,
                                                      DateTime accomodationDate,
                                                      DateTime releaseDate,
                                                      bool allInclusive,
                                                      bool breakfast,
                                                      IEnumerable<ClientData> clients,
                                                      ApplicationUser user)
        {
            var room = await dbContext.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return null;
            }

            if (!await AreDatesAcceptable(roomId, accomodationDate, releaseDate))
            {
                return null;
            }

            if (clients.Count() + 1 > room.Capacity)
            {
                return null;
            }

            var price = await CalculatePriceForNight(room, clients, allInclusive, breakfast) * (releaseDate-accomodationDate).TotalDays;

            var reservation = new Reservation
            {
                AccommodationDate = accomodationDate,
                AllInclusive = allInclusive,
                Breakfast = breakfast,
                Price = price,
                Room = room,
                ReleaseDate = releaseDate,
                Clients = clients,
                User = user,
            };

            this.dbContext.Reservations.Add(reservation);
            await this.dbContext.SaveChangesAsync();

            return reservation;
        }

        /// <summary>
        /// Update reservation data
        /// </summary>
        /// <param name="id">The reservation id</param>
        /// <param name="roomId">The room id</param>
        /// <param name="accomodationDate">The reservation accomodation date</param>
        /// <param name="releaseDate">The reservation release date </param>
        /// <param name="allInclusive">Reservation's order all inclusive</param>
        /// <param name="breakfast">Reservation's order breakfast</param>
        /// <param name="clients">The room's clients</param>
        /// <param name="user">The room renter</param>
        /// <returns>Task representing the success of the update operation</returns>
        public async Task<bool> UpdateReservation(string id,
                                                  DateTime accomodationDate,
                                                  DateTime releaseDate,
                                                  bool allInclusive,
                                                  bool breakfast,
                                                  IEnumerable<ClientData> clients,
                                                  ApplicationUser user)
        {
            var reservation = await dbContext.Reservations.AsNoTracking().Include(x => x.User).Include(x=>x.Room).FirstOrDefaultAsync(x => x.Id == id);

            var room = await dbContext.Rooms.FirstOrDefaultAsync(x => x.Reservations.Any(y => y.Id == id));

            var areDateAcceptable = await AreDatesAcceptable(room.Id, accomodationDate, releaseDate, id);
            var isCapacityInRange = clients.Count() + 1 <= room.Capacity;
            var isUserAuthorizedToUpdate = reservation.User.Id == user.Id ||
                                             dbContext.UserRoles.Any(x => x.UserId == user.Id &&
                                              x.RoleId != dbContext.Roles.First(a => a.Name == "User").Id);

            if (!isUserAuthorizedToUpdate || !isCapacityInRange || !areDateAcceptable)
            {
                return false;
            }

            var price = await CalculatePriceForNight(room, clients, allInclusive, breakfast) * (releaseDate - accomodationDate).TotalDays;

            var newReservation = new Reservation
            {
                Id = id,
                AccommodationDate = accomodationDate,
                AllInclusive = allInclusive,
                Breakfast = breakfast,
                Price = price,
                ReleaseDate = releaseDate,
                Room=room,
                Clients = clients,
                User = user
            };

            dbContext.Reservations.Update(newReservation);
            await this.dbContext.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Removes reservation from the database
        /// </summary>
        /// <param name="id">The reservation to delete id</param>
        /// <returns>Task with the reservation deletion result</returns>
        public async Task<bool> DeleteReservation(string id)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(id);

            if (reservation != null)
            {
                this.dbContext.ClientData.RemoveRange(this.dbContext.ClientData
                                                     .Where(x => x.Reservation.Id == reservation.Id));

                this.dbContext.Reservations.Remove(reservation);
                await this.dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <summary>
        /// Finds reservation with the searched id
        /// </summary>
        /// <typeparam name="T">Data class to map reservation data to</typeparam>
        /// <param name="id">Reservation id to search for</param>
        /// <returns>Task with the reservation data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<T> GetReservation<T>(string id)
        {
            return await this.dbContext.Reservations.AsNoTracking().Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Finds the user's reservation
        /// </summary>
        /// <typeparam name="T">Data class to map reservation data to</typeparam>
        /// <param name="userId">The user who made the reservation id</param>
        /// <returns>Task with the reservations data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetReservationsForUser<T>(string userId)
        {
            return await this.dbContext.Reservations.AsNoTracking()
                                                    .Where(x => x.User.Id == userId)
                                                    .OrderByDescending(x => x.AccommodationDate)
                                                    .ProjectTo<T>().ToListAsync();
        }

        /// <summary>
        /// Finds the user's reservations according to specified pagination rules
        /// </summary>
        /// <typeparam name="T">Data class to map reservation data to</typeparam>
        /// <param name="userId">The user who made the reservation id</param>
        /// <param name="page">The number of current page</param>
        /// <param name="elementsOnPage">The number of reservations on the page</param>
        /// <returns>Task with the reservations data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetForUserOnPage<T>(string userId, int page, int elementsOnPage)
        {
            return await GetReservationsForUser<T>(userId).GetPageItems(page, elementsOnPage);
        }

        /// <summary>
        /// Verifies and updates clients data for existing reservation
        /// </summary>
        /// <param name="reservationId">Reservation id</param>
        /// <param name="clients">Updated clients list</param>
        /// <returns>Task with the clients data list result</returns>
        public async Task<IEnumerable<ClientData>> UpdateClientsForReservation(string reservationId,
                                                                               IEnumerable<ClientData> clients)
        {
            var reservation = await dbContext.Reservations.AsNoTracking()
                                                          .Include(x => x.Room)
                                                          .FirstOrDefaultAsync(x => x.Id == reservationId);
            var initialClients = await dbContext.ClientData.Where(x => x.Reservation.Id == reservationId)
                                                           .ToListAsync();

            //if (clients.Count() + 1 > reservation.Room.Capacity) - Unnecessary

            var deletedClients = initialClients.Where(x => !clients.Select(u => u.Id).Contains(x.Id)).ToList();

            if (deletedClients?.Any() ?? false)
            {
                dbContext.ClientData.RemoveRange(deletedClients);
            }

            var newClients = clients.Where(x => !initialClients.Select(u => u.Id)
                                                               .Contains(x.Id))
                                                               .ToList();

            if (newClients?.Any() ?? false)
            {
                foreach (var cl in newClients)
                {
                    cl.ReservationId = reservation.Id;
                    if (string.IsNullOrWhiteSpace(cl.Id))
                    {
                        cl.Id = Guid.NewGuid().ToString();
                    }
                }

                dbContext.ClientData.AddRange(newClients);
            }

            var clientsToUpdate = clients.Where(x => !newClients.Select(u => u.Id).Contains(x.Id) && x.Id != null).ToList();

            if (clientsToUpdate?.Any() ?? false)
            {
                foreach (var cl in newClients)
                {
                    cl.ReservationId = reservation.Id;
                }
                dbContext.ClientData.UpdateRange(clientsToUpdate);
            }

            await dbContext.SaveChangesAsync();

            return clients;
        }

        /// <summary>
        /// Finds all rooms in the database
        /// </summary>
        /// <typeparam name="T">Data class to map room data to</typeparam>
        /// <returns>Task with all reservations data parsed to <typeparamref name="T"/>
        /// object or null if not found</returns>
        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.dbContext.Reservations.AsNoTracking().OrderBy(x => x.ReleaseDate).ProjectTo<T>().ToListAsync();
        }


        /// <summary>
        /// Finds the count of all reservations in the database
        /// </summary>
        /// <returns>Task with the all reservations count result</returns>
        public async Task<int> CountAllReservations()
        {
            return await this.dbContext.Reservations.AsNoTracking().CountAsync();
        }
    }
}
