using Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IReservationService
    {
        public Task<Reservation> AddReservation(string roomId,
                                                DateTime accomodationDate,
                                                DateTime releaseDate,
                                                bool allInclusive,
                                                bool breakfast,
                                                IEnumerable<ClientData> clients,
                                                ApplicationUser user);
        public Task UpdateReservation(string id,
                                            string roomId,
                                            DateTime accomodationDate,
                                            DateTime releaseDate,
                                            bool allInclusive,
                                            bool breakfast,
                                            IEnumerable<ClientData> clients,
                                            ApplicationUser user);
        public Task DeleteReservation(string id);
        public Task<T> GetReservation<T>(string id);
        public Task<IEnumerable<T>> GetReservationsForUser<T>(string userId);
        public Task<IEnumerable<T>> GetForUserOnPage<T>(string userId, int page, int elementsOnPage);
        public Task<IEnumerable<ClientData>> UpdateClientsForReservation(string reservationId, IEnumerable<ClientData> clients);
    }
}
