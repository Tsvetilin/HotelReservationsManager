using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Services.Common;
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

        public ReservationsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddReservation(double price, DateTime accomodationDate, DateTime releaseDate, bool allInclusive, bool breakfast, IEnumerable<ClientData> clients)
        {
            var reservation = new Reservation
            {
                AccommodationDate = accomodationDate,
                AllInclusive = allInclusive,
                Breakfast = breakfast,
                Price = price,
                ReleaseDate = releaseDate,
                Clients = clients,
            };

            this.dbContext.Reservations.Add(reservation);
            await this.dbContext.SaveChangesAsync();
        }

        public async Task UpdateReservation(string id, double price, DateTime accomodationDate, DateTime releaseDate, bool allInclusive, bool breakfast, IEnumerable<ClientData> clients)
        {
            var reservation = new Reservation
            {
                Id = id,
                AccommodationDate = accomodationDate,
                AllInclusive = allInclusive,
                Breakfast = breakfast,
                Price = price,
                ReleaseDate = releaseDate,
                Clients = clients,
            };

            if (reservation != null)
            {
                this.dbContext.Reservations.Update(reservation);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task DeleteReservation(string id)
        {
            var reservation = await this.dbContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                this.dbContext.Reservations.Remove(reservation);
                await this.dbContext.SaveChangesAsync();
            }
        }

        public async Task<T> GetReservation<T>(string id)
        {
            return await this.dbContext.Reservations.Where(x=>x.Id==id).ProjectTo<T>().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetReservationsForUser<T>(string userId)
        {
            return await this.dbContext.Reservations.Where(x => x.User.Id == userId).OrderByDescending(x => x.AccommodationDate).ProjectTo<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetForUserOnPage<T>(string userId, int page, int elementsOnPage)
        {
            return await GetReservationsForUser<T>(userId).GetPageItems(page, elementsOnPage);
        }
    }
}
