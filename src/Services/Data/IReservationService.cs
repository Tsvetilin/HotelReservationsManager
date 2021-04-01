using Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IReservationService
    {
        public Task AddReservation(double price, DateTime accomodationDate, DateTime releaseDate, bool allInclusive, bool breakfast, IEnumerable<ClientData> clients);
        public Task UpdateReservation(string id, double price, DateTime accomodationDate, DateTime releaseDate, bool allInclusive, bool breakfast, IEnumerable<ClientData> clients);
        public Task DeleteReservation(string id);
        public Task<T> GetReservation<T>(string id);
        public Task<IEnumerable<T>> GetReservationsForUser<T>(string userId);
        public Task<IEnumerable<T>> GetForUserOnPage<T>(string userId, int page, int elementsOnPage);
    }
}
