using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Common;
using Services.Data;
using Services.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
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

        private async Task<bool> AreDatesAcceptable(string roomId, DateTime accomodationDate, DateTime releaseDate, string reservationId=null)
        {
            if (accomodationDate >= releaseDate || accomodationDate < DateTime.Today)
            {
                return false;
            }
            
            var reservationPeriods = await dbContext.
                                           Reservations.
                                           Where(x => x.Room.Id == roomId).
                                           Select(x => new Tuple<DateTime, DateTime>
                                                        (x.AccommodationDate, x.ReleaseDate).
                                                        ToValueTuple()).
                                          ToListAsync();

            if (!string.IsNullOrWhiteSpace(reservationId))
            {
                var reservation = await dbContext.Reservations.FirstOrDefaultAsync(x => x.Id == reservationId);
                reservationPeriods.Remove((reservation.AccommodationDate,reservation.ReleaseDate));
            }

            return !reservationPeriods.Any(x =>
                (x.Item1 > accomodationDate && x.Item1 < releaseDate) ||
                (x.Item2 > accomodationDate && x.Item2 < releaseDate));
        }

        private async Task<double> CalculatePrice(Room room, IEnumerable<ClientData> clients, bool allInclusive, bool breakfast)
        {
            var price =
                clients.Count(x => x.IsAdult) * room.AdultPrice +
                clients.Count(x => !x.IsAdult) * room.ChildrenPrice +
                room.AdultPrice;

            if (allInclusive)
            {
                price += double.Parse((await settingService.GetAsync($"{nameof(Reservation.AllInclusive)}Price")).Value);
            }
            else if (breakfast)
            {
                price += double.Parse((await settingService.GetAsync($"{nameof(Reservation.Breakfast)}Price")).Value);
            }

            return price;
        }

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

            if (await AreDatesAcceptable(roomId,accomodationDate ,releaseDate))
            {
                return null;
            }

            if(clients.Count()+1>room.Capacity)
                    {
                return null;
            }

            var price = await CalculatePrice(room, clients, allInclusive, breakfast);

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

        public async Task<bool> UpdateReservation(string id,
                                            DateTime accomodationDate,
                                            DateTime releaseDate,
                                            bool allInclusive,
                                            bool breakfast,
                                            IEnumerable<ClientData> clients,
                                            ApplicationUser user)
        {
            var reservation = await dbContext.Reservations.AsNoTracking().Include(x => x.User).FirstOrDefaultAsync(x => x.Id == id);

            var room = await dbContext.Rooms.AsNoTracking().FirstOrDefaultAsync(x => x.Reservations.Any(y => y.Id == id));

            var areDateAcceptable = await AreDatesAcceptable(room.Id, accomodationDate, releaseDate, id);
            var isCapacityInRange = clients.Count() + 1 <= room.Capacity;
            var isUserAuthorizedToUpdate = !(reservation.User.Id == user.Id ||
                                             !dbContext.UserRoles.Any(x => x.UserId == user.Id &&
                                              x.RoleId == dbContext.Roles.First(a => a.Name == "User").Id));

            if (!isUserAuthorizedToUpdate || !isCapacityInRange || !areDateAcceptable)
            {
                return false;
            }

            var price = await CalculatePrice(room, clients, allInclusive, breakfast);

            var newReservation = new Reservation
            {
                Id = id,
                AccommodationDate = accomodationDate,
                AllInclusive = allInclusive,
                Breakfast = breakfast,
                Price = price,
                ReleaseDate = releaseDate,
                Room = room,
                Clients = clients,
                User = user
            };

            dbContext.Entry(reservation).CurrentValues.SetValues(newReservation);
            await this.dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteReservation(string id)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                this.dbContext.ClientData.RemoveRange(this.dbContext.ClientData.Where(x => x.Reservation.Id == reservation.Id));
                this.dbContext.Reservations.Remove(reservation);
                await this.dbContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<T> GetReservation<T>(string id)
        {
            return await this.dbContext.Reservations.Where(x => x.Id == id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetReservationsForUser<T>(string userId)
        {
            return await this.dbContext.Reservations.Where(x => x.User.Id == userId).OrderByDescending(x => x.AccommodationDate).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetForUserOnPage<T>(string userId, int page, int elementsOnPage)
        {
            return await GetReservationsForUser<T>(userId).GetPageItems(page, elementsOnPage);
        }

        public async Task<IEnumerable<ClientData>> UpdateClientsForReservation(string reservationId, IEnumerable<ClientData> clients)
        {
            var reservation = await dbContext.Reservations.FindAsync(reservationId);
            var initialClients = await dbContext.ClientData.Where(x => x.Reservation.Id == reservationId).ToListAsync();

            if (initialClients.Count() > clients.Count())
            {
                var deletedClients = initialClients.Where(x => !clients.Select(u => u.Id).Contains(x.Id)).ToList();
                dbContext.ClientData.RemoveRange(deletedClients);
            }

            var newClients = clients.Where(x => !initialClients.Select(u => u.Id).Contains(x.Id) && x.Id != null).ToList();
            if (newClients?.Any() ?? false)
            {
                dbContext.ClientData.AddRange(newClients);
            }

            var clientsToUpdate = clients.Where(x => !newClients.Select(u => u.Id).Contains(x.Id) && x.Id != null);
            if (clientsToUpdate?.Any() ?? false)
            {
                dbContext.ClientData.UpdateRange(clientsToUpdate);
            }

            await dbContext.SaveChangesAsync();

            return clients;
        }

        public async Task<IEnumerable<T>> GetAll<T>()
        {
            return await this.dbContext.Reservations.OrderBy(x => x.ReleaseDate).ProjectTo<T>().ToListAsync();
        }

        public async Task<int> CountAllReservations()
        {
            return await this.dbContext.Reservations.CountAsync();
        }
    }
}
