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

        //TODO: allincl+breakf
        public async Task<Reservation> AddReservation(string roomId,
                                                      DateTime accomodationDate,
                                                      DateTime releaseDate,
                                                      bool allInclusive,
                                                      bool breakfast,
                                                      IEnumerable<ClientData> clients,
                                                      ApplicationUser user)
        {
            var room = await dbContext.Rooms.FindAsync(roomId);

            var price =
                clients.Count(x => x.IsAdult) * room.AdultPrice +
                clients.Count(x => !x.IsAdult) * room.ChildrenPrice + 
                room.AdultPrice;

            if (allInclusive)
            {
                price +=  double.Parse((await settingService.GetAsync($"{nameof(Reservation.AllInclusive)}Price")).Value);
            }
            else if (breakfast)
            {
                price += double.Parse((await settingService.GetAsync($"{nameof(Reservation.Breakfast)}Price")).Value);
            }

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

        public async Task UpdateReservation(string id,
                                            DateTime accomodationDate,
                                            DateTime releaseDate,
                                            bool allInclusive,
                                            bool breakfast,
                                            IEnumerable<ClientData> clients,
                                            ApplicationUser user)
        {
            var reservation = await dbContext.Reservations.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==id);
            //TODO client.count + user <= capacity
            //TODO accomodationDate > dateTime.now
            //TODO room free period
            //TODO releaseDate > accomodation date
            //TODO accomodation date > today
            //TODO room free period excluded previous reservation period
            //TODO return if successfully updated
            //TODO check if user is same
            var room = await dbContext.Rooms.AsNoTracking().FirstOrDefaultAsync(x=>x.Reservations.Any(y=>y.Id==id));

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
        }

        public async Task DeleteReservation(string id)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                this.dbContext.ClientData.RemoveRange(this.dbContext.ClientData.Where(x => x.Reservation.Id == reservation.Id));
                this.dbContext.Reservations.Remove(reservation);
                await this.dbContext.SaveChangesAsync();
            }
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
    }
}
